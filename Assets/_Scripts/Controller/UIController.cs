using System;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(-200)]
public class UIController : MonoBehaviour
{
    [Header("UIの保存場所")]
    [SerializeField] private UIData m_uiData;

    [Header("ポーズを判定する")]
    [SerializeField] private PoseJudgeController m_poseJudgeController;

    //aa
    //一回表示の管理用
    private bool isPoseShown = false;

    //固定値
    private const int success = 0;
    private const int approaching = 3;
    private const int failure = 6;
    private const int wating = 9;

    public Action<int> PoseJudgeFrame;

    private bool once = true;
    private float time = 0;

    [SerializeField] private GameObject uiPrefab1;
    [SerializeField] private GameObject uiPrefab2;

    [SerializeField] private Transform canvas;

    public void UIAnimation(PoseFlow poseFlow, CSVDataPoseFlow pose, float seconds, int specialFrame)
    {

        Debug.Log("[数値1]" + pose.PoseID);
        //開始時間
        if (!isPoseShown && seconds >= pose.start && seconds < pose.end)
        {
            /*
            if(specialFrame <= 2)
            {
                Show(pose.PoseID + wating);
                Show(pose.PoseID + approaching);
                isPoseShown = true;
                Debug.Log("[数値4]" + pose.PoseID);
            }
            */
            
            if (pose.PoseID == 1)
            {
                GameObject obj1 = Instantiate(uiPrefab1, canvas);
                GameObject obj2 = Instantiate(uiPrefab2, canvas);

                RectTransform rect1 = obj1.GetComponent<RectTransform>();
                RectTransform rect2 = obj2.GetComponent<RectTransform>();
                rect1.anchoredPosition = new Vector2(200, 100);
                rect2.anchoredPosition = new Vector2(-200, 100);

                rect1.sizeDelta = new Vector2(400, 400);
                rect2.sizeDelta = new Vector2(400, 400);

                Debug.Log("[数値3]" + pose.PoseID);

               // obj1.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
              //  obj2.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);

                obj1.SetActive(true);
                obj2.SetActive(true);

                isPoseShown = true;
            }
            Debug.Log("[数値2]" + pose.PoseID);
            
        }

       
        /*
        // 縮小(通常フレーム)
        if (specialFrame <= 2 && seconds >= pose.start && seconds < pose.end)
        {
            ScaleDown(pose.PoseID + approaching);

            //イベント実行　当たり判定
            PoseJudgeFrame?.Invoke(pose.PoseID);
        }
        */
        
        // 三人称(通常フレーム)
        if (pose.PoseID == 1 && seconds >= pose.start && seconds < pose.end)
        {
            /*
            if(once)
            {
                time = seconds;
                once = false;
            }


            if (seconds >= (time + 5.0f))
            {

            }
            //*/
          


            pose.PoseID = 0;
            //イベント実行　当たり判定
            PoseJudgeFrame?.Invoke(pose.PoseID);
            pose.PoseID = 2;
            //イベント実行　当たり判定
            PoseJudgeFrame?.Invoke(pose.PoseID);

            pose.PoseID = 1;
        }
    
    


        //完璧成功時
        if (m_poseJudgeController.GetisPose() && m_poseJudgeController.PoseJudge_Normal(pose.PoseID + approaching, pose.PoseID + wating, m_uiData))
        {
            Show(pose.PoseID + success);
            Hide(pose.PoseID + wating);
            Hide(pose.PoseID + approaching);
        }

        //通常成功時
        if (m_poseJudgeController.GetisPose() && m_poseJudgeController.PoseJudge_Perfect(pose.PoseID + approaching, pose.PoseID + wating, m_uiData))
        {
            Show(pose.PoseID + success);
            Hide(pose.PoseID + wating);
            Hide(pose.PoseID + approaching);
        }

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
            //once = true;

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
        Debug.Log("[数値]" + _uinumber);
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
