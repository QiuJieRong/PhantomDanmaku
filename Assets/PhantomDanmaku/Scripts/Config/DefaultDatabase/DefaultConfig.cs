using System.Collections.Generic;
using PhantomDanmaku.Runtime;
using Sirenix.OdinInspector;

namespace PhantomDanmaku.Config
{
    public class DefaultConfig : SerializedConfig
    {
        [LabelText("玩家默认属性字典")]
        public Dictionary<State, StateValue> DefaultPlayerStateDic;
    }
}