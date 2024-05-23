using PhantomDanmaku.Runtime;

namespace PhantomDanmaku
{
    public partial class GameEntry
    {
        public static BattleComponent Battle;
        
        private static void InitCustomComponents()
        {
            Battle = MyFramework.Runtime.GameEntry.GetComponent<BattleComponent>();
        }
    }
}