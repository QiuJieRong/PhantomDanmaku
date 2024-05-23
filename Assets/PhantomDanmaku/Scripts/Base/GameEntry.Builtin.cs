using MyFramework.Runtime;
using PhantomDanmaku.Runtime.UI;

namespace PhantomDanmaku.Runtime
{
    public partial class GameEntry
    {
        public static UIComponent UI;

        public static ObjectPoolComponent ObjectPool;

        public static SaveComponent Save;

        public static EventCenterComponent EventCenter;

        public static SoundComponent Sound;
        
        private static void InitBuiltinComponents()
        {
            UI = MyFramework.Runtime.GameEntry.GetComponent<UIComponent>();
            ObjectPool = MyFramework.Runtime.GameEntry.GetComponent<ObjectPoolComponent>();
            Save = MyFramework.Runtime.GameEntry.GetComponent<SaveComponent>();
            EventCenter = MyFramework.Runtime.GameEntry.GetComponent<EventCenterComponent>();
            Sound = MyFramework.Runtime.GameEntry.GetComponent<SoundComponent>();
        }
    }
}