using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PoseJudgeController : MonoBehaviour
{
    [Header("UIの保存場所")]
    [SerializeField] private UIController m_uiController;

    [Header("CSVのデータリスト")]
    private List<CSVPoseData> poseDatas;

    [Header("ポーズの判定")]
    private bool isPose;

    ///<summary>
    ///現在のポーズが成功しているかの判定
    ///</summary>
    public bool GetisPose() { return isPose; }

    //オブザーバー
    private void OnEnable()
    {
        m_uiController.PoseJudgeFrame += PoseJudge;
    }

    //オブザーバー
    private void OnDisable()
    {
        m_uiController.PoseJudgeFrame -= PoseJudge;
    }

    public void PoseJudge(int poseID)
    {

        ///指定されたポーズデータを入れる
        CSVPoseData pose = poseDatas[poseID];

        /*
        ///ポーズのスコア計算
        Score = scoreCalculator.TotalScore(
            Mathf.Abs(pose.LeftShoulderRotation[0] - AngleDataManager.Instance.angleData.angle[1]), Mathf.Abs(pose.RightShoulderRotation[0] - AngleDataManager.Instance.angleData.angle[3]),
            Mathf.Abs(pose.LeftelbowRotation[0] - AngleDataManager.Instance.angleData.angle[0]), Mathf.Abs(pose.RightelbowRotation[0] - AngleDataManager.Instance.angleData.angle[2])
            );
        */
        ///ポーズの判定
        if (AngleDataManager.Instance.angleData.angle[0] <= (pose.LeftelbowRotation[0] + pose.LeftelbowRotation[1]) &&
            AngleDataManager.Instance.angleData.angle[0] >= (pose.LeftelbowRotation[0] - pose.LeftelbowRotation[1]) &&
            AngleDataManager.Instance.angleData.angle[1] <= (pose.LeftShoulderRotation[0] + pose.LeftShoulderRotation[1]) &&
            AngleDataManager.Instance.angleData.angle[1] >= (pose.LeftShoulderRotation[0] - pose.LeftShoulderRotation[1]) &&
            AngleDataManager.Instance.angleData.angle[2] <= (pose.RightelbowRotation[0] + pose.RightelbowRotation[1]) &&
            AngleDataManager.Instance.angleData.angle[2] >= (pose.RightelbowRotation[0] - pose.RightelbowRotation[1]) &&
            AngleDataManager.Instance.angleData.angle[3] <= (pose.RightShoulderRotation[0] + pose.RightShoulderRotation[1]) &&
            AngleDataManager.Instance.angleData.angle[3] >= (pose.RightShoulderRotation[0] - pose.RightShoulderRotation[1])
            )
        {
            isPose = true;

        }
        else
        {
            isPose = false;

        }

    }

    /// <summary>
    ///パーフェクトのポーズ判定 
    /// <summary>
    public bool PoseJudge_Perfect(int _uinumber_approaching, int _uinumber_wating , UIData _uiData)
    {
        return _uiData.getUI(_uinumber_wating).transform.localScale.x >= _uiData.getUI(_uinumber_approaching).transform.localScale.x - 0.1f &&
               _uiData.getUI(_uinumber_wating).transform.localScale.x <= _uiData.getUI(_uinumber_approaching).transform.localScale.x + 0.1f;

    }

    /// <summary>
    ///通常のポーズ判定 
    /// <summary>
    public bool PoseJudge_Normal(int _uinumber_approaching, int _uinumber_wating, UIData _uiData)
    {
        return _uiData.getUI(_uinumber_wating).transform.localScale.x >= _uiData.getUI(_uinumber_approaching).transform.localScale.x - 0.3f &&
               _uiData.getUI(_uinumber_wating).transform.localScale.x <= _uiData.getUI(_uinumber_approaching).transform.localScale.x + 0.3f;

    }

 
}
