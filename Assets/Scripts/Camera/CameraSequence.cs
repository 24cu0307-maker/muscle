using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 複数のCameraShotPresetを順番に再生する設定です。
/// </summary>
[CreateAssetMenu(
    fileName = "Sequence_New",
    menuName = "Camera/Sequence")]
public sealed class CameraSequence : ScriptableObject
{
    [Header("この演出の説明")]
    [TextArea(2, 4)]                                               
    [SerializeField]
    private string memo = "どの場面で使う演出かを書きます。";

    [Header("再生設定")]
    [Tooltip("ONなら演出中にTime.timeScaleを0にします。")]
    [SerializeField]
    private bool pauseGameTime = true;

    [Tooltip("ONなら演出終了後に通常カメラへ戻ります。")]
    [SerializeField]
    private bool returnToGameplayCamera = true;

    [Tooltip("ONならStopSequenceが呼ばれるまで繰り返します。")]
    [SerializeField]
    private bool loop = false;

    [Header("再生するカット。上から順番に再生します")]
    [SerializeField]
    private List<CameraShotPreset> shots =
        new List<CameraShotPreset>();

    public string Memo => memo;
    public bool PauseGameTime => pauseGameTime;
    public bool ReturnToGameplayCamera => returnToGameplayCamera;
    public bool Loop => loop;
    public IReadOnlyList<CameraShotPreset> Shots => shots;
}
