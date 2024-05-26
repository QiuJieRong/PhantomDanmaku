namespace PhantomDanmaku.Runtime.UI
{
    using UnityEngine.UI;
    using UnityEngine;

    public partial class TalentToggle : UIGroupBase
    {
        private Toggle m_TalentToggleToggle;
        private Image m_TalentToggleImage;
        private RectTransform m_ChildrenRectTransform;
        private RectTransform m_ActiveRectTransform;
        private RectTransform m_LockRectTransform;
        protected override void InstallField()
        {
            base.InstallField();
            m_TalentToggleToggle = transform.Find("").GetComponent<Toggle>();
            m_TalentToggleImage = transform.Find("").GetComponent<Image>();
            m_ChildrenRectTransform = transform.Find("Children/").GetComponent<RectTransform>();
            m_ActiveRectTransform = transform.Find("Active/").GetComponent<RectTransform>();
            m_LockRectTransform = transform.Find("Lock/").GetComponent<RectTransform>();
        }
    }
}