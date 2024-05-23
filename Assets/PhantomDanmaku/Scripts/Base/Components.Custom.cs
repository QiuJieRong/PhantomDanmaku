using MyFramework.Runtime;

namespace PhantomDanmaku.Runtime
{
    public partial class Components
    {
        public static BattleComponent Battle;

        public static LauncherComponent Launcher;
        
        private static void InitCustomComponents()
        {
            Battle = GameEntry.GetComponent<BattleComponent>();
            Launcher = GameEntry.GetComponent<LauncherComponent>();
        }
    }
}