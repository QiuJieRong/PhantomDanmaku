namespace PhantomDanmaku.Runtime.UI
{
    using TMPro;
    using UnityEngine.UI;

    public partial class LevelEndUIForm : UIFormBase
    {
        private TextMeshProUGUI m_MessageTextMeshProUGUI;
        private Button m_NextLevelBtnButton;
        private Button m_AgainBtnButton;
        private Button m_BackHallBtnButton;
        protected override void InstallField()
        {
            base.InstallField();
            m_MessageTextMeshProUGUI = transform.Find("Background/Message/").GetComponent<TextMeshProUGUI>();
            m_NextLevelBtnButton = transform.Find("Background/Buttons/NextLevelBtn/").GetComponent<Button>();
            m_AgainBtnButton = transform.Find("Background/Buttons/AgainBtn/").GetComponent<Button>();
            m_BackHallBtnButton = transform.Find("Background/Buttons/BackHallBtn/").GetComponent<Button>();
        }
    }
}