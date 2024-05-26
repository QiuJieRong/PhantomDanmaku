namespace PhantomDanmaku.Runtime.UI
{
    using UnityEngine.UI;
    using TMPro;
    using UnityEngine;

    public partial class TalentUIForm : UIFormBase
    {
        private Button m_CloseBtnButton;
        private Button m_UpgradeBtnButton;
        private TextMeshProUGUI m_TalentDescTextMeshProUGUI;
        private TextMeshProUGUI m_TalentNameTextMeshProUGUI;
        private RectTransform m_ContentRectTransform;
        private ToggleGroup m_ContentToggleGroup;
        private RectTransform m_Root1RectTransform;
        private RectTransform m_Root2RectTransform;
        protected override void InstallField()
        {
            base.InstallField();
            m_CloseBtnButton = transform.Find("Background/CloseBtn/").GetComponent<Button>();
            m_UpgradeBtnButton = transform.Find("Background/UpgradeBtn/").GetComponent<Button>();
            m_TalentDescTextMeshProUGUI = transform.Find("Background/TalentDesc/").GetComponent<TextMeshProUGUI>();
            m_TalentNameTextMeshProUGUI = transform.Find("Background/TalentName/").GetComponent<TextMeshProUGUI>();
            m_ContentRectTransform = transform.Find("Background/Scroll View/Viewport/Content/").GetComponent<RectTransform>();
            m_ContentToggleGroup = transform.Find("Background/Scroll View/Viewport/Content/").GetComponent<ToggleGroup>();
            m_Root1RectTransform = transform.Find("Background/Scroll View/Viewport/Content/Root1/").GetComponent<RectTransform>();
            m_Root2RectTransform = transform.Find("Background/Scroll View/Viewport/Content/Root2/").GetComponent<RectTransform>();
        }
    }
}