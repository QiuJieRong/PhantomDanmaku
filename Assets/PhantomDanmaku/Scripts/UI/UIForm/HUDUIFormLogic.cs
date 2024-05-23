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

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Refresh(userData);
        }

        private void Refresh(object userData)
        {
            if (userData is EntityBase player)
            {
                RefreshState(m_HpFillImage, m_HpTextMeshProUGUI, player.Hp, player.MaxHp);
                RefreshState(m_ShieldFillImage, m_ShieldTextMeshProUGUI, player.Shield, player.MaxShield);
                RefreshState(m_EnergyFillImage, m_EnergyTextMeshProUGUI, player.CurEnergy, player.MaxEnergy);
            }
        }

        private void RefreshState(Image fillImage,TextMeshProUGUI text,int curValue,int maxValue)
        {
            fillImage.fillAmount = (float)curValue / maxValue;
            text.text = $"{curValue}/{maxValue}";
        }
    }
}