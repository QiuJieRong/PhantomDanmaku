using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PhantomDanmaku.Runtime.UI
{
    public partial class EndUIForm
    {
        public override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_AgainButtonButton.onClick.AddListener(async () =>
            {
                GameEntry.ObjectPool.Clear();
                GameEntry.UI.Close(this);
                await SceneManager.UnloadSceneAsync("SampleScene");
                GameEntry.Battle.StartBattle(0, 0).Forget();
            });
            
            m_ExitButtonButton.onClick.AddListener(() =>
            {
                Application.Quit();
            });
        }
    }
}