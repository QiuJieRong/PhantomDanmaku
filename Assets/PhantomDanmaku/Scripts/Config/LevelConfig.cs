using Sirenix.OdinInspector;

namespace PhantomDanmaku.Config
{
    public class LevelConfig : SerializedConfig
    {
        [LabelText("关卡Id")]
        public int Id;

        [LabelText("关卡名字")]
        public string Name;
        
        [LabelText("关卡描述")]
        public string Desc;

        [Title("关卡开始事件")]
        public Event OnLevelStart;
        
        [Title("关卡结束事件")]
        public Event OnLevelEnd;
    }
}