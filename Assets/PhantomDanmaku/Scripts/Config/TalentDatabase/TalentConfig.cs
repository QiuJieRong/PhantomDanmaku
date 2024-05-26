using Sirenix.OdinInspector;

namespace PhantomDanmaku.Config
{
    public class TalentConfig : SerializedConfig
    {
        [LabelText("天赋名字")]
        public string TalentName;
        
        [LabelText("天赋描述")]
        public string TalentDesc;
        
        [LabelText("前置天赋")]
        public TalentConfig PreTalent;

        [LabelText("技能点消耗")]
        public int SkillPointCost;

        [LabelText("效果")]
        public TalentEffect TalentEffect;
    }
}