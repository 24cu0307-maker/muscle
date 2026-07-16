using UnityEngine;

/// <summary>
/// UI ButtonやゲームイベントからCameraSequenceを再生するための入口です。
/// </summary>
public sealed class CameraSequenceTrigger : MonoBehaviour
{
    [SerializeField]
    private PoseCameraDirector director;

    [SerializeField]
    private CameraSequence sequence;

    [Tooltip("テスト用です。通常はOFFにします。")]
    [SerializeField]
    private bool playOnStart = false;

    private void Start()
    {
        if (playOnStart) { Play(); }
    }

    public void Play()
    {
        if (director == null || sequence == null)
        {
            Debug.LogError("CameraSequenceTriggerの設定が不足しています。", this);
            return;
        }
        director.PlaySequence(sequence);
    }

    public void Stop()
    {
        if (director != null)
        {
            director.StopSequence();
        }
    }
}
