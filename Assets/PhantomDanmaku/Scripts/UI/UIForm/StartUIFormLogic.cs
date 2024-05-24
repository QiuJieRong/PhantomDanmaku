using Cysharp.Threading.Tasks;
using PhantomDanmaku.Runtime.System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PhantomDanmaku.Runtime.UI
{
    public partial class StartUIForm
    {
        public override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_StartButton.onClick.AddListener(() =>
            {
                Components.UI.Close(this);
               
                //进入大厅
                SceneManager.LoadScene("HallScene", LoadSceneMode.Additive);
            });

            m_ExitButton.onClick.AddListener(() =>
            {
                Application.Quit();
            });
        }
    }
}