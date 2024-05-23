using MyFramework.Runtime;

namespace PhantomDanmaku.Runtime
{
    public partial class Components
    {
        public static BattleComponent Battle;

        public static LauncherComponent Launcher;

        public static InputComponent Input;
        
        private static void InitCustomComponents()
        {
            Battle = GameEntry.GetComponent<BattleComponent>();
            Launcher = GameEntry.GetComponent<LauncherComponent>();
            Input = GameEntry.GetComponent<InputComponent>();
        }
    }
}