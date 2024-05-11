using PhantomDanmaku.Runtime.UI;

namespace PhantomDanmaku
{
    public partial class GameEntry
    {
        public static UIComponent UI;

        public static BattleComponent Battle;
        
        private static void InitBuiltinComponents()
        {
            UI = MyFramework.Runtime.GameEntry.GetComponent<UIComponent>();
            Battle = MyFramework.Runtime.GameEntry.GetComponent<BattleComponent>();
        }
    }
}