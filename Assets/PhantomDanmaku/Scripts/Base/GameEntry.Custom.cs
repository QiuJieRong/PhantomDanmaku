namespace PhantomDanmaku.Runtime
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