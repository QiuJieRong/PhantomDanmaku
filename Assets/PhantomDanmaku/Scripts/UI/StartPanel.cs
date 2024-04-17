using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PhantomDanmaku.Scripts.System;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : PanelBase
{
    void Start()
    {
        var startButton = GetControl<Button>("StartButton");

        startButton.onClick.AddListener(()=>
        {
            UIMgr.Instance.HidePanel("StartPanel");
            SceneMgr.Instance.LoadScene("SampleScene");
        });

        GetControl<Button>("ExitButton").onClick.AddListener(()=>
        {
            Application.Quit();
        });
        
        //加载配置，暂时放在这里
        PhantomSystem.Instance.LoadConfig().Forget();
    }

    void Update()
    {
        
    }
}
