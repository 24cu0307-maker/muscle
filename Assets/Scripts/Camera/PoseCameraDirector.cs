using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

//CameraSequenceを読み込み、ポーズ用カメラを動かす
public sealed class PoseCameraDirector : MonoBehaviour
{
    [Header("Cinemachine")]
    [SerializeField]
    private CinemachineCamera gameplayCamera;

    [SerializeField]
    private CinemachineCamera poseCamera;

    [SerializeField]
    private CinemachineBrain cinemachineBrain;

    [Header("モデルと注視点")]
    [Tooltip("モデル全体の一番上のオブジェクトです。")]
    [SerializeField]
    private Transform characterRoot;

    [Tooltip("モデルの見た目上の正面を示すTransformです。青いZ矢印を顔の向きへ合わせます。")]
    [SerializeField]
    private Transform frontReference;

    [SerializeField]
    private Transform waistTarget;

    [SerializeField]
    private Transform chestTarget;

    [SerializeField]
    private Transform faceTarget;

    [Header("Priority")]
    [SerializeField]
    private int gameplayPriority = 10;

    [SerializeField]
    private int activePosePriority = 100;

    [SerializeField]
    private int inactivePosePriority = 0;

    [Header("初心者用テスト")]
    [Tooltip("ここへSequenceを入れるとPキーでテストできます。")]
    [SerializeField]
    private CameraSequence testSequence;

    
    [SerializeField]
    private Key testPlayKey = Key.P;

    [SerializeField]
    private Key stopKey = Key.O;
    
    [SerializeField]
    private bool testPlay = false;

    private bool testbool = true;

    private Coroutine playRoutine;
    private bool changedTimeScale;
    private float savedTimeScale = 1.0f;

    private CameraState currentState;
    private Vector3 currentLookPoint;
    private bool hasCurrentState;

    public bool IsPlaying => playRoutine != null;

    public void SetTestPlay(bool isplay)
    {
        testPlay = isplay;
    }

    private void Awake()
    {
        if (cinemachineBrain != null)
        {
            //Time.timeScaleが0でもCinemachineを更新します。
            cinemachineBrain.IgnoreTimeScale = true;
        }
        SetGameplayCameraActive();
    }

    private void Update()
    {
        
        if (Keyboard.current[testPlayKey].wasPressedThisFrame)
        {
            if (IsPlaying) { StopSequence(); }
            else { PlaySequence(testSequence); }
        }
        if (Keyboard.current[stopKey].wasPressedThisFrame) { StopSequence(); }
        

        if (testPlay && testbool)
        {
            if (IsPlaying) { StopSequence(); }
            else { PlaySequence(testSequence); }
            testbool = false;
        }

    }

    /*
     指定したカメラ演出を再生します。
     ゲーム側やUI Buttonから呼び出せます。
    */
    public void PlaySequence(CameraSequence sequence)
    {
        if (!ValidateReferences() || !ValidateSequence(sequence)) { return; }
        StopSequence();
        playRoutine = StartCoroutine(PlaySequenceRoutine(sequence));
    }

    //演出を停止して通常カメラへ戻します。
    public void StopSequence()
    {
        if (playRoutine != null)
        {
            StopCoroutine(playRoutine);
            playRoutine = null;
        }

        RestoreTimeScale();
        SetGameplayCameraActive();
        hasCurrentState = false;
    }

    private IEnumerator PlaySequenceRoutine(CameraSequence sequence)
    {
        if (sequence.PauseGameTime)
        {
            savedTimeScale = Time.timeScale;
            Time.timeScale = 0.0f;
            changedTimeScale = true;
        }

        CameraShotPreset firstShot = FindFirstValidShot(sequence);
        Transform firstTarget = GetTarget(firstShot.TargetPoint);

        currentState = BuildStartState(firstShot);
        currentLookPoint = firstTarget.position;
        hasCurrentState = true;

        ApplyState(currentState, currentLookPoint);
        SetPoseCameraActive();

        do
        {
            for (int i = 0; i < sequence.Shots.Count; i++)
            {
                CameraShotPreset shot = sequence.Shots[i];

                if (shot == null)
                {
                    continue;
                }

                Transform target = GetTarget(shot.TargetPoint);
                CameraState startState = BuildStartState(shot);
                CameraState endState = BuildEndState(shot);

                if (hasCurrentState && shot.TransitionDuration > 0.0f)
                {
                    yield return MoveToShotStart(currentState, currentLookPoint, startState, target, shot.TransitionDuration);
                }
                else
                {
                    currentState = startState;
                    currentLookPoint = target.position;
                    ApplyState(currentState, currentLookPoint);
                }

                yield return MoveInsideShot(startState, endState, target, shot.MoveDuration, shot.MoveCurve);
                yield return HoldShot(endState, target, shot.HoldDuration);
            }
        }
        while (sequence.Loop);

        playRoutine = null;
        RestoreTimeScale();

        if (sequence.ReturnToGameplayCamera)
        {
            SetGameplayCameraActive();
            hasCurrentState = false;
        }
    }

    private IEnumerator MoveToShotStart(CameraState fromState, Vector3 fromLookPoint, CameraState toState, Transform toTarget, float duration)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float rate = Mathf.Clamp01(elapsed / duration);
            rate = Mathf.SmoothStep(0.0f, 1.0f, rate);

            CameraState state = CameraState.Lerp(fromState, toState, rate);
            Vector3 lookPoint = Vector3.Lerp(fromLookPoint, toTarget.position, rate);

            ApplyState(state, lookPoint);

            currentState = state;
            currentLookPoint = lookPoint;

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        currentState = toState;
        currentLookPoint = toTarget.position;
        ApplyState(currentState, currentLookPoint);
    }

    private IEnumerator MoveInsideShot(CameraState startState, CameraState endState, Transform target, float duration, AnimationCurve moveCurve)
    {
        if (duration <= 0.0f)
        {
            currentState = endState;
            currentLookPoint = target.position;
            ApplyState(currentState, currentLookPoint);
            yield break;
        }

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float rate = Mathf.Clamp01(elapsed / duration);
            float curvedRate = moveCurve != null ? moveCurve.Evaluate(rate) : rate;

            CameraState state = CameraState.Lerp(startState, endState, curvedRate);

            ApplyState(state, target.position);

            currentState = state;
            currentLookPoint = target.position;

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        currentState = endState;
        currentLookPoint = target.position;
        ApplyState(currentState, currentLookPoint);
    }

    private IEnumerator HoldShot(CameraState state, Transform target, float duration)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            currentLookPoint = target.position;
            ApplyState(state, currentLookPoint);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        currentState = state;
        currentLookPoint = target.position;
    }

    private void ApplyState(CameraState state, Vector3 lookPoint)
    {
        Quaternion frontRotation = GetFrontRotation();

        // Front Referenceの青いZ矢印側を、モデルの見た目上の正面として扱います。
        // Yaw 0度は正面、+90度はモデル右側、-90度はモデル左側です。
        // Pitchはプラスで上側、マイナスで下側から映します。
        Vector3 orbitDirection = frontRotation * Quaternion.Euler(-state.pitch, state.yaw, 0.0f) * Vector3.forward;
        Vector3 modelRight = frontRotation * Vector3.right;
        Vector3 cameraPosition = lookPoint + orbitDirection * state.distance + Vector3.up * state.height + modelRight * state.side;
        Vector3 lookDirection = lookPoint - cameraPosition;

        if (lookDirection.sqrMagnitude < 0.0001f) { return; }

        Quaternion cameraRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        poseCamera.transform.SetPositionAndRotation(cameraPosition, cameraRotation);

        LensSettings lens = poseCamera.Lens;
        lens.FieldOfView = state.fieldOfView;
        lens.Dutch = state.dutch;
        poseCamera.Lens = lens;
    }

    /*
      Front Referenceの向きから、水平な正面回転を作ります。
      モデルのFBXが180度回転していても、Front Referenceだけ直せば
      Shot PresetのYaw 0度が必ず正面になります。
     */
    private Quaternion GetFrontRotation()
    {
        Vector3 flatForward = Vector3.ProjectOnPlane(frontReference.forward, Vector3.up);
        if (flatForward.sqrMagnitude < 0.0001f)
        {
            flatForward = Vector3.ProjectOnPlane(characterRoot.forward, Vector3.up);
        }
        if (flatForward.sqrMagnitude < 0.0001f)
        {
            flatForward = Vector3.forward;
        }
        return Quaternion.LookRotation(flatForward.normalized, Vector3.up);
    }

    private Transform GetTarget(PoseCameraTargetPoint targetPoint)
    {
        switch (targetPoint)
        {
            case PoseCameraTargetPoint.Waist:
                return waistTarget;

            case PoseCameraTargetPoint.Face:
                return faceTarget;

            case PoseCameraTargetPoint.Chest:
            default:
                return chestTarget;
        }
    }

    private CameraShotPreset FindFirstValidShot(CameraSequence sequence)
    {
        for (int i = 0; i < sequence.Shots.Count; i++)
        {
            if (sequence.Shots[i] != null)
            {
                return sequence.Shots[i];
            }
        }
        return null;
    }

    private CameraState BuildStartState(CameraShotPreset shot)
    {
        return new CameraState(shot.StartYaw, shot.StartPitch, shot.StartDistance, shot.StartHeight, shot.StartSide, shot.StartFieldOfView, shot.StartDutch);
    }

    private CameraState BuildEndState(CameraShotPreset shot)
    {
        return new CameraState(shot.EndYaw, shot.EndPitch, shot.EndDistance, shot.EndHeight, shot.EndSide, shot.EndFieldOfView, shot.EndDutch);
    }

    private void SetGameplayCameraActive()
    {
        if (gameplayCamera != null)
        {
            gameplayCamera.Priority = gameplayPriority;
        }

        if (poseCamera != null)
        {
            poseCamera.Priority = inactivePosePriority;
        }
    }

    private void SetPoseCameraActive()
    {
        gameplayCamera.Priority = gameplayPriority;
        poseCamera.Priority = activePosePriority;
    }

    private void RestoreTimeScale()
    {
        if (!changedTimeScale) { return; }
        Time.timeScale = savedTimeScale;
        changedTimeScale = false;
    }

    private bool ValidateReferences()
    {
        if (gameplayCamera == null || poseCamera == null || cinemachineBrain == null || characterRoot == null ||
            frontReference == null || waistTarget == null || chestTarget == null || faceTarget == null)
        {
            Debug.LogError( "PoseCameraDirectorの参照設定が不足しています。" + "Inspectorの赤い空欄を確認してください。", this);
            return false;
        }
        return true;
    }

    private bool ValidateSequence(CameraSequence sequence)
    {
        if (sequence == null)
        {
            Debug.LogError( "再生するCameraSequenceが設定されていません。", this);
            return false;
        }

        if (FindFirstValidShot(sequence) == null)
        {
            Debug.LogError("CameraSequenceにShot Presetが入っていません。", sequence);
            return false;
        }
        return true;
    }

    private void OnDisable()
    {
        if (playRoutine != null)
        {
            StopCoroutine(playRoutine);
            playRoutine = null;
        }

        RestoreTimeScale();
    }

    private void OnDrawGizmosSelected()
    {
        DrawTargetGizmo(waistTarget, 0.08f);
        DrawTargetGizmo(chestTarget, 0.10f);
        DrawTargetGizmo(faceTarget, 0.12f);
        DrawFrontReferenceGizmo();
    }

    private void DrawFrontReferenceGizmo()
    {
        if (frontReference == null) { return; }

        Vector3 start = chestTarget != null ? chestTarget.position : frontReference.position;
        Vector3 flatForward = Vector3.ProjectOnPlane(frontReference.forward, Vector3.up);

        if (flatForward.sqrMagnitude < 0.0001f) { return; }

        Vector3 end = start + flatForward.normalized * 1.0f;
        Gizmos.DrawLine(start, end);
        Gizmos.DrawWireSphere(end, 0.08f);
    }

    private static void DrawTargetGizmo(Transform target, float radius)
    {
        if (target != null)
        {
            Gizmos.DrawWireSphere(target.position, radius);
        }
    }

    private struct CameraState
    {
        public readonly float yaw;
        public readonly float pitch;
        public readonly float distance;
        public readonly float height;
        public readonly float side;
        public readonly float fieldOfView;
        public readonly float dutch;
        public CameraState(float yaw, float pitch, float distance, float height, float side, float fieldOfView, float dutch)
        {
            this.yaw = yaw;
            this.pitch = pitch;
            this.distance = distance;
            this.height = height;
            this.side = side;
            this.fieldOfView = fieldOfView;
            this.dutch = dutch;
        }

        public static CameraState Lerp(CameraState from, CameraState to, float rate)
        {
            return new CameraState(Mathf.LerpAngle(from.yaw, to.yaw, rate), Mathf.LerpAngle(from.pitch, to.pitch, rate), Mathf.Lerp(from.distance, to.distance, rate),
                Mathf.Lerp(from.height, to.height, rate), Mathf.Lerp(from.side, to.side, rate), Mathf.Lerp(from.fieldOfView, to.fieldOfView, rate), Mathf.LerpAngle(from.dutch, to.dutch, rate));
        }
    }
}
