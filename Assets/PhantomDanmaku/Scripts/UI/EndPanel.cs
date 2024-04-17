using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPanel : PanelBase
{
    // Start is called before the first frame update
    void Start()
    {
        GetControl<Button>("AgainButton").onClick.AddListener(() =>
        {
            PoolMgr.Instance.Clear();
            UIMgr.Instance.HidePanel("EndPanel");
            SceneMgr.Instance.LoadScene("SampleScene");
        });
        GetControl<Button>("ExitButton").onClick.AddListener(()=>
        {
            Application.Quit();
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
