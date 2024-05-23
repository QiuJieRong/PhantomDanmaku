using MyFramework.Runtime;
using PhantomDanmaku.Runtime.UI;

namespace PhantomDanmaku.Runtime
{
    public partial class Components
    {
        public static UIComponent UI;

        public static ObjectPoolComponent ObjectPool;

        public static SaveComponent Save;

        public static EventCenterComponent EventCenter;

        public static SoundComponent Sound;
        
        private static void InitBuiltinComponents()
        {
            UI = GameEntry.GetComponent<UIComponent>();
            ObjectPool = GameEntry.GetComponent<ObjectPoolComponent>();
            Save = GameEntry.GetComponent<SaveComponent>();
            EventCenter = GameEntry.GetComponent<EventCenterComponent>();
            Sound = GameEntry.GetComponent<SoundComponent>();
        }
    }
}