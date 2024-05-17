using MyFramework.Runtime;
using PhantomDanmaku.Runtime.UI;

namespace PhantomDanmaku
{
    public partial class GameEntry
    {
        public static UIComponent UI;

        public static BattleComponent Battle;

        public static ObjectPoolComponent ObjectPool;

        public static SaveComponent Save;

        public static EventCenterComponent EventCenter;
        
        private static void InitBuiltinComponents()
        {
            UI = MyFramework.Runtime.GameEntry.GetComponent<UIComponent>();
            Battle = MyFramework.Runtime.GameEntry.GetComponent<BattleComponent>();
            ObjectPool = MyFramework.Runtime.GameEntry.GetComponent<ObjectPoolComponent>();
            Save = MyFramework.Runtime.GameEntry.GetComponent<SaveComponent>();
            EventCenter = MyFramework.Runtime.GameEntry.GetComponent<EventCenterComponent>();
        }
    }
}