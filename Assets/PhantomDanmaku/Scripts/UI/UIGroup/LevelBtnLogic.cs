namespace PhantomDanmaku.Runtime.UI
{
    public partial class LevelBtn
    {
        private int m_LevelIdx;
        public override void OnInit(object userData)
        {
            base.OnInit(userData);
            if (userData is int levelIdx)
            {
                m_LevelIdx = levelIdx;
                m_LevelBtnButton.onClick.AddListener(() =>
                {
                    SendUIMessage("SelectLevel", m_LevelIdx);
                });
                m_IndexTextMeshProUGUI.text = $"{m_LevelIdx + 1}";
            }
        }
    }
}