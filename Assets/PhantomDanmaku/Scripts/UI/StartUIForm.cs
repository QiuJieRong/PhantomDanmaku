using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = PhantomDanmaku.GameEntry;

public class StartUIForm : UIFormLogic
{
    public Button startButton;

    public Button exitButton;
    void Start()
    {
        startButton.onClick.AddListener(()=>
        {
            GameEntry.UI.CloseUIForm(UIForm);
            SceneManager.LoadScene("SampleScene");
        });

        exitButton.onClick.AddListener(Application.Quit);
        
    }
}
