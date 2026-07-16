using GameFlowTemplate;
using System;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

public sealed class InGameManager : MonoBehaviour
{
    //インスタンス化
    //public static InGameManager Instance { get; private set; }


    [Header("Effectの管理")]
    [SerializeField] private EffectSystem m_effectSystem;
    //[Header("UIの管理")]
    //[SerializeField] private UIManamager m_uiManamager;
    [Header("UIの操作")]
    [SerializeField] private UIController m_uiController;
    [Header("終了の時間")]
    [SerializeField] private float m_endtimer;


    private float GameTimeSeconds;                  //現在のゲーム時間
    private int PoseMaxCount = 20;            //ポーズ数を設定

    private PoseFlow poseFlow;  　　　　  //ポーズ順の管理

    private CSVDataPoseFlow pose;

    //private int PoseFram = 0;

    public Action<PoseFlow, CSVDataPoseFlow, float> PoseFrame;


    private void Awake()
    {

        //ゲームを開始する
        //GameManagerで管理している
        //Timerとスコアをリセット
        //Timerの開始と状態の切り替え
        GameManager.Instance.StartGame();

        // CSVのデータをPoseFlowへ渡す
        poseFlow = new PoseFlow(ExcelLoader.Instance.excelPoseTimeFlowLoader.GetCSVDatas());


    }

    private void Update()
    {
        //現在のゲーム時間の更新
        UpdateTime();

        //現在のポーズを取得
        pose = poseFlow.CurrentPose();

        //現在のポーズのフレームを実行　衝突判定とフレームUIの管理
        m_uiController.UIAnimation(poseFlow, pose, GameTimeSeconds);


    }


    /// <summary>
    /// 現在のゲーム時間の更新
    /// </summary>
    private void UpdateTime()
    {
        GameTimeSeconds = GameManager.Instance.GetTimeManager().GameTimeSeconds;
    }

    /// <summary>
    /// ゲームを継続するか
    /// </summary>
    private void ContinuingGame()
    {
        if (PoseMaxCount == 0)
        {
            GameManager.Instance.FinishGame();

        }

        PoseMaxCount--;
    }

}
