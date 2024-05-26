using Cysharp.Threading.Tasks;
using PhantomDanmaku.Runtime.UI;

namespace PhantomDanmaku.Runtime
{
    public class TalentBook : InteroperableObjectBase
    {
        public override bool CanInteract => true;
        public override void Interact()
        {
            //打开天赋界面
            Components.UI.Open<TalentUIForm>(null).Forget();
        }
    }
}