namespace PhantomDanmaku.Runtime.UI
{
    using UnityEngine.UI;

    public partial class StartUIForm : UIFormBase
    {
        private Button m_StartButton;
        private Button m_ExitButton;
        protected override void InstallField()
        {
            base.InstallField();
            m_StartButton = transform.Find("Start/").GetComponent<Button>();
            m_ExitButton = transform.Find("Exit/").GetComponent<Button>();
        }
    }
}