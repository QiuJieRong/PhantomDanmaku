using System.Collections.Generic;
using PhantomDanmaku.Runtime;
using Sirenix.OdinInspector;

namespace PhantomDanmaku.Config
{
    public class TalentEffect
    {
        [LabelText("属性修正器")]
        public List<Modifier> Modifiers;

        public void Invoke(PlayerData playerData)
        {
            foreach (var modifier in Modifiers)
            {
                var stateValue = playerData.GetStateValue(modifier.State);
                stateValue.AddModifier(modifier);
            }
        }
    }
}