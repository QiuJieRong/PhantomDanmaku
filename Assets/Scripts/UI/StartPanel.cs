using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : PanelBase
{
    void Start()
    {
        GetControl<Button>("StartButton").onClick.AddListener(()=>
        {
            UIMgr.Instance.HidePanel("StartPanel");
            SceneMgr.Instance.LoadScene("SampleScene");
        });
        GetControl<Button>("ExitButton").onClick.AddListener(()=>
        {
            Application.Quit();
        });
        
    }

    void Update()
    {
        
    }
}
