namespace PhantomDanmaku.Runtime.UI
{
    using UnityEngine.UI;
    using TMPro;

    public partial class HUDUIForm : UIFormBase
    {
        private Image m_HpFillImage;
        private Image m_ShieldFillImage;
        private TextMeshProUGUI m_HpTextMeshProUGUI;
        private TextMeshProUGUI m_ShieldTextMeshProUGUI;
        protected override void InstallField()
        {
            base.InstallField();
            m_HpFillImage = transform.Find("StateBackground/Image_HpContainer/HpFill/").GetComponent<Image>();
            m_ShieldFillImage = transform.Find("StateBackground/Image_ShieldContainer/ShieldFill/").GetComponent<Image>();
            m_HpTextMeshProUGUI = transform.Find("StateBackground/Image_HpContainer/Hp/").GetComponent<TextMeshProUGUI>();
            m_ShieldTextMeshProUGUI = transform.Find("StateBackground/Image_ShieldContainer/Shield/").GetComponent<TextMeshProUGUI>();
        }
    }
}