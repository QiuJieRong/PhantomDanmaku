using Cysharp.Threading.Tasks;
using PhantomDanmaku.Runtime.UI;

namespace PhantomDanmaku.Runtime
{
    public class PortalEnter : PortalBase
    {
        public override bool CanInteract => true;
        public override void Interact()
        {
            //打开选关界面
            Components.UI.Open<LevelUIForm>(null).Forget();
        }
    }
}