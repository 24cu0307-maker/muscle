using GameFlowTemplate;
using System;
using UnityEngine;
using UnityEngine.Rendering;

public class UIManamager : MonoBehaviour
{
    //インスタンス化
    public static UIManamager Instance { get; private set; }

  
    [Header("UIの操作")]
    [SerializeField] private UIController m_uiController;



}
