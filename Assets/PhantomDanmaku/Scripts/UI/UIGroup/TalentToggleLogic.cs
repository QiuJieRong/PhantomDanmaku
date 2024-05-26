using PhantomDanmaku.Config;
using UnityEngine;
using UnityEngine.UI;

namespace PhantomDanmaku.Runtime.UI
{
    public partial class TalentToggle
    {
        private TalentConfig m_TalentConfig;

        public Toggle Toggle => m_TalentToggleToggle;

        public Transform ChildrenTransform => m_ChildrenRectTransform;

        public override void OnInit(object userData)
        {
            base.OnInit(userData);
            if (userData is TalentConfig talentConfig)
            {
                m_TalentConfig = talentConfig;
            }

            m_TalentToggleToggle.onValueChanged.AddListener(isOn =>
            {
                if (isOn)
                {
                    SendUIMessage("SelectTalent", m_TalentConfig); 
                }
            });
            
            Refresh();
        }

        private void Refresh()
        {
            m_TalentNameTextMeshProUGUI.text = m_TalentConfig.TalentName;
        }
    }
}