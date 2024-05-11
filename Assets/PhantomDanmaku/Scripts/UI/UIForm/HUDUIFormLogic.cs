using TMPro;
using UnityEngine.UI;

namespace PhantomDanmaku.Runtime.UI
{
    public partial class HUDUIForm
    {
        public override void OnInit(object userData)
        {
            base.OnInit(userData);
            RegisterUIMessage("RefreshHUDUIForm",Refresh);
        }

        private void Refresh(object userData)
        {
            if (userData is EntityBase player)
            {
                RefreshState(m_HpFillImage, m_HpTextMeshProUGUI, player.CurHp, player.Hp);
                RefreshState(m_ShieldFillImage, m_ShieldTextMeshProUGUI, player.CurShield, player.Shield);
                RefreshState(m_HpFillImage, m_HpTextMeshProUGUI, player.CurEnergy, player.Energy);
            }
        }

        private void RefreshState(Image fillImage,TextMeshProUGUI text,int curValue,int maxValue)
        {
            fillImage.fillAmount = (float)curValue / maxValue;
            text.text = $"{curValue}/{maxValue}";
        }
    }
}