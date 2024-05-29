using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PhantomDanmaku.Runtime.UI
{
    public partial class HUDUIForm
    {
        private RenderTexture m_RenderTexture;
        public override void OnInit(object userData)
        {
            base.OnInit(userData);
            RegisterUIMessage("RefreshHUDUIForm",Refresh);
            m_RenderTexture = new RenderTexture(1920, 1080, 1, RenderTextureFormat.Default);
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Refresh(userData);
            //获得地图摄像机
            var mapCamera = GameObject.Find("MapCamera").GetComponent<Camera>();
            mapCamera.targetTexture = m_RenderTexture;
            m_MapRawImage.texture = m_RenderTexture;
            mapCamera.enabled = true;
        }

        private void Refresh(object userData)
        {
            if (userData is EntityBase player)
            {
                RefreshState(m_HpFillImage, m_HpTextMeshProUGUI, player.CurHp, player.MaxHp);
                RefreshState(m_ShieldFillImage, m_ShieldTextMeshProUGUI, player.CurShield, player.MaxShield);
                RefreshState(m_EnergyFillImage, m_EnergyTextMeshProUGUI, player.CurEnergy, player.MaxEnergy);
            }
        }

        private void RefreshState(Image fillImage,TextMeshProUGUI text,float curValue,float maxValue)
        {
            fillImage.fillAmount = curValue / maxValue;
            text.text = $"{curValue}/{maxValue}";
        }
    }
}