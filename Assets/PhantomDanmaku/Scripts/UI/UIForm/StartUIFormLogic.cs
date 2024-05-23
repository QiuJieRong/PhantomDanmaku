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
                GameEntry.UI.Close(this);
                
                BattleComponent.StartBattle(0, 0).Forget();
            });

            m_ExitButton.onClick.AddListener(() =>
            {
                Application.Quit();
            });
        
            //加载配置，暂时放在这里
            PhantomSystem.Instance.LoadConfig().Forget();
        }
    }
}