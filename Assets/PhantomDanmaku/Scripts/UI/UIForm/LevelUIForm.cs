namespace PhantomDanmaku.Runtime.UI
{
    using UnityEngine.UI;
    using TMPro;
    using UnityEngine;

    public partial class LevelUIForm : UIFormBase
    {
        private Button m_PreChapterBtnButton;
        private TextMeshProUGUI m_ChapterTitleTextMeshProUGUI;
        private Button m_NextChapterBtnButton;
        private RectTransform m_ContentRectTransform;
        private TextMeshProUGUI m_LevelNameTextMeshProUGUI;
        private TextMeshProUGUI m_LevelDescTextMeshProUGUI;
        private Button m_StartBtnButton;
        private Button m_CloseBtnButton;
        protected override void InstallField()
        {
            base.InstallField();
            m_PreChapterBtnButton = transform.Find("Background/PreChapterBtn/").GetComponent<Button>();
            m_ChapterTitleTextMeshProUGUI = transform.Find("Background/ChapterTitle/").GetComponent<TextMeshProUGUI>();
            m_NextChapterBtnButton = transform.Find("Background/NextChapterBtn/").GetComponent<Button>();
            m_ContentRectTransform = transform.Find("Background/Levels/Viewport/Content/").GetComponent<RectTransform>();
            m_LevelNameTextMeshProUGUI = transform.Find("Background/LevelName/").GetComponent<TextMeshProUGUI>();
            m_LevelDescTextMeshProUGUI = transform.Find("Background/LevelDesc/LevelDesc/").GetComponent<TextMeshProUGUI>();
            m_StartBtnButton = transform.Find("Background/StartBtn/").GetComponent<Button>();
            m_CloseBtnButton = transform.Find("Background/CloseBtn/").GetComponent<Button>();
        }
    }
}