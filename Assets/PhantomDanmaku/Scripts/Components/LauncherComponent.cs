using Cysharp.Threading.Tasks;
using MyFramework.Runtime;
using PhantomDanmaku.Runtime.System;
using PhantomDanmaku.Runtime.UI;

namespace PhantomDanmaku.Runtime
{
    public class LauncherComponent : GameFrameworkComponent
    {
        private async void Start()
        {
            //显示加载界面
            await Components.UI.Open<LoadingUIForm>(null);
            //注册自身后开始加载配置
            await PhantomSystem.Instance.LoadConfig();
            //加载配置完成，关闭加载界面，打开开始界面
            Components.UI.Close<LoadingUIForm>();
            Components.UI.Open<StartUIForm>(null).Forget();
        }
    }
}