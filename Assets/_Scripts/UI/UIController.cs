using System;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("UIの保存場所")]
    [SerializeField] private UIData m_uiData;


    //一回表示の管理用
    private bool isPoseShown = false;

    //固定値
    private const int success = 0;
    private const int approaching = 3;
    private const int failure = 6;
    private const int wating = 9;

    public Action<int> PoseJudgeFrame;



    public void UIAnimation(PoseFlow poseFlow, CSVDataPoseFlow pose, float seconds)
    {
        pose = poseFlow.CurrentPose();

        //開始時間
        if (!isPoseShown && seconds >= pose.start && seconds < pose.end)
        {
            Show(pose.PoseID + wating);
            Show(pose.PoseID + approaching);
            isPoseShown = true;

        }

        // 縮小
        if (seconds >= pose.start && seconds < pose.end)
        {
            //イベント実行　当たり判定
            PoseJudgeFrame?.Invoke(pose.PoseID);

            ScaleDown(pose.PoseID + approaching);
        }

        /*
        //成功時
        if (angleJudge.GetisPose() && poseJudge.PoseJudge_Perfect(uIData.getUI(pose.PoseID + approaching), uIData.getUI(pose.PoseID + wating))
)
        {
            m_uiController.Show(uIData.getUI(pose.PoseID + success));
            m_uiController.Hide(uIData.getUI(pose.PoseID + wating));
            m_uiController.Hide(uIData.getUI(pose.PoseID + approaching));
        }
        */


        // 終了時間
        if (seconds >= pose.end && poseFlow.HasNextPose())
        {
            Hide(pose.PoseID + wating);
            Hide(pose.PoseID + approaching);
            Hide(pose.PoseID + success);
            ScaleReset(pose.PoseID + approaching);
            poseFlow.NextPose();

            // 次のポーズ用にリセット
            isPoseShown = false;

        }

    }


    /// <summary>
    ///サイズダウン
    /// <summary>
    public void ScaleDown(int _uinumber)
    {
        m_uiData.getUI(_uinumber).transform.localScale -= Vector3.one * Time.deltaTime * 0.15f;
    }

    /// <summary>
    ///サイズリセット
    /// <summary>
    public void ScaleReset(int _uinumber)
    {
        m_uiData.getUI(_uinumber).transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
    }

    /// <summary>
    ///表示
    /// <summary>
    public void Show(int _uinumber)
    {
        m_uiData.getUI(_uinumber).SetActive(true);
    }

    /// <summary>
    ///非表示
    /// <summary>
    public void Hide(int _uinumber)
    {
        m_uiData.getUI(_uinumber).SetActive(false);
    }

  
}
