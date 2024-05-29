namespace PhantomDanmaku.Runtime.UI
{
    using UnityEngine.UI;
    using TMPro;

    public partial class HUDUIForm : UIFormBase
    {
        private Image m_HpFillImage;
        private TextMeshProUGUI m_HpTextMeshProUGUI;
        private Image m_ShieldFillImage;
        private TextMeshProUGUI m_ShieldTextMeshProUGUI;
        private Image m_EnergyFillImage;
        private TextMeshProUGUI m_EnergyTextMeshProUGUI;
        private RawImage m_MapRawImage;
        protected override void InstallField()
        {
            base.InstallField();
            m_HpFillImage = transform.Find("StateBackground/Image_HpContainer/HpFill/").GetComponent<Image>();
            m_HpTextMeshProUGUI = transform.Find("StateBackground/Image_HpContainer/Hp/").GetComponent<TextMeshProUGUI>();
            m_ShieldFillImage = transform.Find("StateBackground/Image_ShieldContainer/ShieldFill/").GetComponent<Image>();
            m_ShieldTextMeshProUGUI = transform.Find("StateBackground/Image_ShieldContainer/Shield/").GetComponent<TextMeshProUGUI>();
            m_EnergyFillImage = transform.Find("StateBackground/Image_EnergyContainer/EnergyFill/").GetComponent<Image>();
            m_EnergyTextMeshProUGUI = transform.Find("StateBackground/Image_EnergyContainer/Energy/").GetComponent<TextMeshProUGUI>();
            m_MapRawImage = transform.Find("MapBackground/Map/").GetComponent<RawImage>();
        }
    }
}