using GameFlowTemplate;
using System;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

public sealed class InGameManager : MonoBehaviour
{



    //[Header("Effectの管理")]
    //[SerializeField] private EffectSystem m_effectSystem;
    //[Header("UIの管理")]
    //[SerializeField] private UIManamager m_uiManamager;
    [Header("UIの操作")]
    [SerializeField] private UIController m_uiController;
    [Header("UIの保存場所")]
    [SerializeField] private ExcelLoader m_excelLoader;
    [Header("gameManager")]
    [SerializeField] private GameManager m_gameManager;

    [Header("観客")]
    [SerializeField] private AudienceController m_audienceController;

    [Header("終了の時間")]
    [SerializeField] private float m_endtimer;

    private float GameTimeSeconds;                  //現在のゲーム時間
    private int PoseMaxCount = 20;            //ポーズ数を設定

    private PoseFlow poseFlow;  　　　　  //ポーズ順の管理

    private CSVDataPoseFlow pose;

    private int m_SpecialFrame = -1;

    public Action<PoseFlow, CSVDataPoseFlow, float> PoseFrame;



    public void Start()
    {

        //ゲームを開始する
        //GameManagerで管理している
        //Timerとスコアをリセット
        //Timerの開始と状態の切り替え
        m_gameManager.StartGame();

        // CSVのデータをPoseFlowへ渡す
        poseFlow = new PoseFlow(m_excelLoader.excelPoseTimeFlowLoader.GetCSVDatas());


    }

    private void Update()
    {
        //if (m_endtimer <= GameTimeSeconds) { m_gameManager.FinishGame(); }

        //現在のゲーム時間の更新
        UpdateTime();

        //現在のポーズを取得
        pose = poseFlow.CurrentPose();

        Debug.Log("[数値]" + pose.PoseID);

        //現在のポーズのフレームを実行　衝突判定とフレームUIの管理
        m_uiController.UIAnimation(poseFlow, pose, GameTimeSeconds, m_SpecialFrame);

        //フレームの場合
        switch (pose.PoseID)
        {
            //通常フレーム
            case 0:
                //カメラの移動実行

                break;

            //通常フレーム
            case 1:
                //カメラの移動実行

                break;
            //通常フレーム
            case 2:
                //カメラの移動実行

                break;

            //3人称視点
            case 10:
                //カメラの移動実行

                //観客起動

                break;

            //溜めてタイミング
            case 11:
                //カメラの移動実行

                //観客起動

                break;

            //キープタイミング
            case 12:
                //カメラの移動実行

                break;
        }

    }


    /// <summary>
    /// 現在のゲーム時間の更新
    /// </summary>
    private void UpdateTime()
    {
        GameTimeSeconds = m_gameManager.GetTimeManager().GameTimeSeconds;
    }

    /// <summary>
    /// ゲームを継続するか
    /// </summary>
    private void ContinuingGame()
    {
        if (PoseMaxCount == 0)
        {
            m_gameManager.FinishGame();

        }

        PoseMaxCount--;
    }

}
