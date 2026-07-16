using UnityEngine;

/// <summary>
/// 1カット分のカメラ設定です。
/// プランナーはこのアセットの数値だけを調整します。
/// </summary>
[CreateAssetMenu(
    fileName = "Shot_New",
    menuName = "Camera/Preset")]
public sealed class CameraShotPreset : ScriptableObject
{
    [Header("このカットの説明")]
    [TextArea(2, 4)]
    [SerializeField]
    private string memo = "何を見せるカメラかを書きます。";

    [Header("注目する場所")]
    [Tooltip("全身はWaist、筋肉はChest、表情はFaceが目安です。")]
    [SerializeField]
    private PoseCameraTargetPoint targetPoint = PoseCameraTargetPoint.Chest;

    [Header("時間")]
    [Tooltip("前のカットから開始位置へ移る時間です。")]
    [Min(0.0f)]
    [SerializeField]
    private float transitionDuration = 0.25f;

    [Tooltip("開始位置から終了位置へ動く時間です。")]
    [Min(0.01f)]
    [SerializeField]
    private float moveDuration = 1.0f;

    [Tooltip("移動が終わったあと、その画角を見せる時間です。")]
    [Min(0.0f)]
    [SerializeField]
    private float holdDuration = 0.4f;

    [Tooltip("カメラ移動の加速と減速を決めるカーブです。")]
    [SerializeField]
    private AnimationCurve moveCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);

    [Header("開始位置")]
    [Tooltip("0度がFront Referenceの青いZ矢印側（正面）、+90度が右側、-90度が左側、180度が背面です。")]
    [Range(-180.0f, 180.0f)]
    [SerializeField]
    private float startYaw = -30.0f;

    [Tooltip("プラスで上側、マイナスで下側から映します。")]
    [Range(-45.0f, 45.0f)]
    [SerializeField]
    private float startPitch = 0.0f;

    [Tooltip("モデルからカメラまでの距離です。")]
    [Range(1.5f, 12.0f)]
    [SerializeField]
    private float startDistance = 4.2f;

    [Tooltip("カメラを上下へずらします。")]
    [Range(-3.0f, 3.0f)]
    [SerializeField]
    private float startHeight = 0.0f;

    [Tooltip("カメラを左右へずらします。")]
    [Range(-3.0f, 3.0f)]
    [SerializeField]
    private float startSide = 0.0f;

    [Tooltip("小さいほど望遠で大きく映ります。")]
    [Range(15.0f, 90.0f)]
    [SerializeField]
    private float startFieldOfView = 45.0f;

    [Tooltip("画面の傾きです。基本は0、強くても5度程度がおすすめです。")]
    [Range(-15.0f, 15.0f)]
    [SerializeField]
    private float startDutch = 0.0f;

    [Header("終了位置")]
    [Range(-180.0f, 180.0f)]
    [SerializeField]
    private float endYaw = 30.0f;

    [Range(-45.0f, 45.0f)]
    [SerializeField]
    private float endPitch = -3.0f;

    [Range(1.5f, 12.0f)]
    [SerializeField]
    private float endDistance = 3.5f;

    [Range(-3.0f, 3.0f)]
    [SerializeField]
    private float endHeight = -0.2f;

    [Range(-3.0f, 3.0f)]
    [SerializeField]
    private float endSide = 0.0f;

    [Range(15.0f, 90.0f)]
    [SerializeField]
    private float endFieldOfView = 36.0f;

    [Range(-15.0f, 15.0f)]
    [SerializeField]
    private float endDutch = 0.0f;

    public string Memo => memo;
    public PoseCameraTargetPoint TargetPoint => targetPoint;
    public float TransitionDuration => transitionDuration;
    public float MoveDuration => moveDuration;
    public float HoldDuration => holdDuration;
    public AnimationCurve MoveCurve => moveCurve;

    public float StartYaw => startYaw;
    public float StartPitch => startPitch;
    public float StartDistance => startDistance;
    public float StartHeight => startHeight;
    public float StartSide => startSide;
    public float StartFieldOfView => startFieldOfView;
    public float StartDutch => startDutch;

    public float EndYaw => endYaw;
    public float EndPitch => endPitch;
    public float EndDistance => endDistance;
    public float EndHeight => endHeight;
    public float EndSide => endSide;
    public float EndFieldOfView => endFieldOfView;
    public float EndDutch => endDutch;
}
