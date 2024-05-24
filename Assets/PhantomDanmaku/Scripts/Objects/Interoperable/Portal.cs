using Cysharp.Threading.Tasks;
using PhantomDanmaku.Runtime;
using PhantomDanmaku.Runtime.UI;

namespace PhantomDanmaku.Scripts.Objects.Interoperable
{
    public class Portal : InteroperableObjectBase
    {
        public override void Interact()
        {
            //打开选关界面
            Components.UI.Open<LevelUIForm>(null).Forget();
        }
    }
}