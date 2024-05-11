namespace PhantomDanmaku.Runtime.UI
{
    using UnityEngine.UI;

    public partial class EndUIForm : UIFormBase
    {
        private Button m_AgainButtonButton;
        private Button m_ExitButtonButton;
        protected override void InstallField()
        {
            base.InstallField();
            m_AgainButtonButton = transform.Find("AgainButton/").GetComponent<Button>();
            m_ExitButtonButton = transform.Find("ExitButton/").GetComponent<Button>();
        }
    }
}