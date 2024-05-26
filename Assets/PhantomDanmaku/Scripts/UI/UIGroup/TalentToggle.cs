namespace PhantomDanmaku.Runtime.UI
{
    using UnityEngine.UI;
    using TMPro;
    using UnityEngine;

    public partial class TalentToggle : UIGroupBase
    {
        private Toggle m_TalentToggleToggle;
        private Image m_TalentToggleImage;
        private TextMeshProUGUI m_TalentNameTextMeshProUGUI;
        private RectTransform m_ChildrenRectTransform;
        protected override void InstallField()
        {
            base.InstallField();
            m_TalentToggleToggle = transform.Find("").GetComponent<Toggle>();
            m_TalentToggleImage = transform.Find("").GetComponent<Image>();
            m_TalentNameTextMeshProUGUI = transform.Find("TalentName/").GetComponent<TextMeshProUGUI>();
            m_ChildrenRectTransform = transform.Find("Children/").GetComponent<RectTransform>();
        }
    }
}