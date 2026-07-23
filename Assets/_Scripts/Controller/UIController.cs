using System;
<<<<<<< HEAD
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEditor.PlayerSettings;
=======
using Unity.VisualScripting;
using UnityEngine;
>>>>>>> 65471bca7e8f2e06018e8515274cb51c715ec805

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

<<<<<<< HEAD
    private GameObject[] m_currentFrame;

    private GameObject[] m_backFrame = new GameObject[3];

    private GameObject m_currentFrameSuccess;
    private GameObject m_currentFrameApproaching;
    private GameObject m_currentFrameFailure;
    private GameObject m_currentFrameWating;

    private bool once = true;
    private float time = 0;

    private const int size = 200;

    /*
    [SerializeField] private GameObject uiPrefab1;
    [SerializeField] private GameObject uiPrefab2;
    */

    [SerializeField] private Transform m_canvas;

    [SerializeField] private Transform m_thirdPersonCanvas;

    public void UIAnimation(PoseFlow poseFlow, CSVDataPoseFlow pose, float seconds)
    {
        Debug.Log(seconds);

        //フレームごとの処理
        switch (pose.PoseID)
        {
            //3人称視点
            case 3:

                //開始時間
                if (!isPoseShown && seconds >= pose.start && seconds < pose.end)
                {
                    m_currentFrame = new GameObject[12];
                    for (int i = 0; i < 12; i++)
                    {
                        int poseID = i / 4;          // 0,0,0,0,1,1,1,1,2,2,2,2
                        int addFrameID = (i % 4) * 3; // 0,3,6,9

                        Vector2 pos = poseID switch
                        {
                            0 => new Vector2(0, 0),
                            1 => new Vector2(800, 0),
                            2 => new Vector2(-800, 0),
                            _ => Vector2.zero
                        };

                        m_currentFrame[i] = CreateFrame(poseID, addFrameID, pos, m_thirdPersonCanvas, new Vector2(500, 500));
                    }


                    for (int i = 0; i < m_backFrame.Length; i++)
                    {
                        Vector2 pos = i switch
                        {
                            0 => new Vector2(0, 0),
                            1 => new Vector2(800, 0),
                            2 => new Vector2(-800, 0),
                            _ => Vector2.zero
                        };
                        m_backFrame[i] = CreateFrame(12, 0, pos, m_thirdPersonCanvas, new Vector2(400, 400));
                        m_backFrame[i].transform.SetAsFirstSibling();
                        Show(m_backFrame[i]);
                    }


                    for (int i = 1; i < m_currentFrame.Length; i += 2)
                    {
                        Show(m_currentFrame[i]);
                    }
                    isPoseShown = true;


                }


                // 縮小(通常フレーム)
                if (seconds >= pose.start && seconds < pose.end)
                {
                    for (int i = 1; i < m_currentFrame.Length; i += 4)
                    {
                        ScaleDown1(m_currentFrame[i]);
                    }

                    //イベント実行　当たり判定
                    for (int i = 0; i < 3; i++)
                    {
                        PoseJudgeFrame?.Invoke(i);
                    }


                }


                for (int poseID = 0; poseID < 3; poseID++)
                {
                    int index = poseID * 4 + 1;

                    if (m_poseJudgeController.GetisPose(poseID) &&
                        m_poseJudgeController.PoseJudge_Normal(
                            m_currentFrame[index],
                            m_currentFrame[index + 2]))
                    {
                        Show(m_currentFrame[index - 1]);
                        Hide(m_currentFrame[index]);
                        Hide(m_currentFrame[index + 2]);


                    }
                }


                for (int poseID = 0; poseID < 3; poseID++)
                {

                    int index = poseID * 4 + 1;

                    if (m_poseJudgeController.GetisPose(poseID) &&
                        m_poseJudgeController.PoseJudge_Perfect(
                            m_currentFrame[index],
                            m_currentFrame[index + 2]))
                    {
                        Show(m_currentFrame[index - 1]);
                        Hide(m_currentFrame[index]);
                        Hide(m_currentFrame[index + 2]);


                    }
                }


                break;

            //溜めてタイミング
            case 4:

                break;

            //キープタイミング
            case 5:

                break;

            //通常フレーム
            case <= 2:

                //開始時間
                if (!isPoseShown && seconds >= pose.start && seconds < pose.end)
                {
                    m_currentFrame = new GameObject[4];
                    for (int i = 0; i < 4; i++)
                    {
                        m_currentFrame[i] = CreateFrame(pose.PoseID, i * 3, Vector2.zero, m_canvas, new Vector2(2400, 2400));


                    }
                    /*
                    m_currentFrameWating = CreateFrame(pose.PoseID, wating, Vector2.zero);
                    m_currentFrameApproaching = CreateFrame(pose.PoseID, approaching, Vector2.zero);
                    m_currentFrameFailure = CreateFrame(pose.PoseID, failure, Vector2.zero);
                    m_currentFrameSuccess = CreateFrame(pose.PoseID, success, Vector2.zero);
                    */
                    Show(m_currentFrame[3]);
                    Show(m_currentFrame[1]);
                    isPoseShown = true;
                }

                // 縮小(通常フレーム)
                if (seconds >= pose.start && seconds < pose.end)
                {
                    ScaleDown(m_currentFrame[1]);


                    //イベント実行　当たり判定
                    PoseJudgeFrame?.Invoke(pose.PoseID);
                }

                //完璧成功時
                if (m_poseJudgeController.GetisPose(pose.PoseID) &&
                    m_poseJudgeController.PoseJudge_Normal(m_currentFrame[1], m_currentFrame[3]))
                {


                    for (int i = 0; i < m_currentFrame.Length; i += 4)
                    {
                        Show(m_currentFrame[i]);

                    }

                    for (int i = 1; i < m_currentFrame.Length; i += 2)
                    {
                        Hide(m_currentFrame[i]);
                    }
                }

                //通常成功時
                if (m_poseJudgeController.GetisPose(pose.PoseID) &&
                    m_poseJudgeController.PoseJudge_Perfect(m_currentFrame[1], m_currentFrame[3]))
                {


                    for (int i = 0; i < m_currentFrame.Length; i += 4)
                    {
                        Show(m_currentFrame[i]);

                    }

                    for (int i = 1; i < m_currentFrame.Length; i += 2)
                    {
                        Hide(m_currentFrame[i]);
                    }
                }
                break;

        }



        /*
=======
    private bool once = true;
    private float time = 0;

    [SerializeField] private GameObject uiPrefab1;
    [SerializeField] private GameObject uiPrefab2;

    [SerializeField] private Transform canvas;

    public void UIAnimation(PoseFlow poseFlow, CSVDataPoseFlow pose, float seconds, int specialFrame)
    {

>>>>>>> 65471bca7e8f2e06018e8515274cb51c715ec805
        Debug.Log("[数値1]" + pose.PoseID);
        //開始時間
        if (!isPoseShown && seconds >= pose.start && seconds < pose.end)
        {
<<<<<<< HEAD
           
            //if(specialFrame <= 2)
=======
            /*
            if(specialFrame <= 2)
>>>>>>> 65471bca7e8f2e06018e8515274cb51c715ec805
            {
                Show(pose.PoseID + wating);
                Show(pose.PoseID + approaching);
                isPoseShown = true;
                Debug.Log("[数値4]" + pose.PoseID);
            }
<<<<<<< HEAD
            
=======
            */
>>>>>>> 65471bca7e8f2e06018e8515274cb51c715ec805
            
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

       
<<<<<<< HEAD
        
      
        
=======
        /*
        // 縮小(通常フレーム)
        if (specialFrame <= 2 && seconds >= pose.start && seconds < pose.end)
        {
            ScaleDown(pose.PoseID + approaching);

            //イベント実行　当たり判定
            PoseJudgeFrame?.Invoke(pose.PoseID);
        }
        */
>>>>>>> 65471bca7e8f2e06018e8515274cb51c715ec805
        
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
<<<<<<< HEAD
            //
          


            //イベント実行　当たり判定
            PoseJudgeFrame?.Invoke(pose.PoseID);
            //イベント実行　当たり判定
            PoseJudgeFrame?.Invoke(pose.PoseID);

        }
    */





=======
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
>>>>>>> 65471bca7e8f2e06018e8515274cb51c715ec805

        // 終了時間
        if (seconds >= pose.end && poseFlow.HasNextPose())
        {
<<<<<<< HEAD

            for (int i = 0; i < m_currentFrame.Length; i++)
            {
                DeleteFrame(m_currentFrame[i]);


            }
            DeleteFrame(m_backFrame[0]);
            DeleteFrame(m_backFrame[1]);
            DeleteFrame(m_backFrame[2]);
            /*
            Hide(m_currentFrameSuccess);
            Hide(m_currentFrameWating);
            Hide(m_currentFrameApproaching);
            ScaleReset(m_currentFrameApproaching);
            */
=======
            Hide(pose.PoseID + wating);
            Hide(pose.PoseID + approaching);
            Hide(pose.PoseID + success);
            ScaleReset(pose.PoseID + approaching);
>>>>>>> 65471bca7e8f2e06018e8515274cb51c715ec805
            poseFlow.NextPose();

            // 次のポーズ用にリセット
            isPoseShown = false;
<<<<<<< HEAD
            once = true;
=======
            //once = true;
>>>>>>> 65471bca7e8f2e06018e8515274cb51c715ec805

        }

    }


    /// <summary>
    ///サイズダウン
    /// <summary>
<<<<<<< HEAD
    public void ScaleDown(GameObject m_uiData)
    {
        m_uiData.transform.localScale -= Vector3.one * Time.deltaTime * 0.05f;
    }

    public void ScaleDown1(GameObject m_uiData)
    {
        m_uiData.transform.localScale -= Vector3.one * Time.deltaTime * 0.05f;
=======
    public void ScaleDown(int _uinumber)
    {
        m_uiData.getUI(_uinumber).transform.localScale -= Vector3.one * Time.deltaTime * 0.15f;
>>>>>>> 65471bca7e8f2e06018e8515274cb51c715ec805
    }

    /// <summary>
    ///サイズリセット
    /// <summary>
<<<<<<< HEAD
    public void ScaleReset(GameObject m_uiData)
    {
        m_uiData.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
=======
    public void ScaleReset(int _uinumber)
    {
        m_uiData.getUI(_uinumber).transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
>>>>>>> 65471bca7e8f2e06018e8515274cb51c715ec805
    }

    /// <summary>
    ///表示
    /// <summary>
<<<<<<< HEAD
    public void Show(GameObject m_uiData)
    {
        m_uiData.SetActive(true);
=======
    public void Show(int _uinumber)
    {
        Debug.Log("[数値]" + _uinumber);
        m_uiData.getUI(_uinumber).SetActive(true);
>>>>>>> 65471bca7e8f2e06018e8515274cb51c715ec805
    }

    /// <summary>
    ///非表示
    /// <summary>
<<<<<<< HEAD
    public void Hide(GameObject m_uiData)
    {
        m_uiData.SetActive(false);
    }


    public GameObject CreateFrame(int _frameID, int _addFrameID, Vector2 _pos, Transform _canvas, Vector2 size)
    {
        GameObject obj = Instantiate(
            m_uiData.getUI(_frameID + _addFrameID),
            _canvas
        );

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchoredPosition = _pos;

        rect.sizeDelta = size;

        return obj;
    }

    public void DeleteFrame(GameObject _uiFrame)
    {
        Destroy(_uiFrame);
    }

=======
    public void Hide(int _uinumber)
    {
        m_uiData.getUI(_uinumber).SetActive(false);
    }

  
>>>>>>> 65471bca7e8f2e06018e8515274cb51c715ec805
}
