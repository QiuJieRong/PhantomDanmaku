namespace PhantomDanmaku.Runtime.UI
{
    using UnityEngine.UI;
    using TMPro;

    public partial class LevelBtn : UIGroupBase
    {
        private Button m_LevelBtnButton;
        private TextMeshProUGUI m_IndexTextMeshProUGUI;
        protected override void InstallField()
        {
            base.InstallField();
            m_LevelBtnButton = transform.Find("").GetComponent<Button>();
            m_IndexTextMeshProUGUI = transform.Find("Index/").GetComponent<TextMeshProUGUI>();
        }
    }
}