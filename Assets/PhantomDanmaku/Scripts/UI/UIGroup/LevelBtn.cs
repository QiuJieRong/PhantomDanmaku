namespace PhantomDanmaku.Runtime.UI
{
    using UnityEngine.UI;
    using TMPro;
    using UnityEngine;

    public partial class LevelBtn : UIGroupBase
    {
        private Button m_LevelBtnButton;
        private TextMeshProUGUI m_IndexTextMeshProUGUI;
        private RectTransform m_LockRectTransform;
        protected override void InstallField()
        {
            base.InstallField();
            m_LevelBtnButton = transform.Find("").GetComponent<Button>();
            m_IndexTextMeshProUGUI = transform.Find("Index/").GetComponent<TextMeshProUGUI>();
            m_LockRectTransform = transform.Find("Lock/").GetComponent<RectTransform>();
        }
    }
}