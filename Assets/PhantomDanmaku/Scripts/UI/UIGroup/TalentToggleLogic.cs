using Cysharp.Threading.Tasks;
using PhantomDanmaku.Config;
using PhantomDanmaku.Runtime.System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace PhantomDanmaku.Runtime.UI
{
    public partial class TalentToggle
    {
        private TalentConfig m_TalentConfig;

        public Toggle Toggle => m_TalentToggleToggle;

        public Transform ChildrenTransform => m_ChildrenRectTransform;

        public override void OnInit(object userData)
        {
            base.OnInit(userData);
            if (userData is TalentConfig talentConfig)
            {
                m_TalentConfig = talentConfig;
            }

            m_TalentToggleToggle.onValueChanged.AddListener(isOn =>
            {
                if (isOn)
                {
                    SendUIMessage("SelectTalent", m_TalentConfig); 
                }
            });
            
            Refresh();
        }

        public async void Refresh()
        {
            var handle = m_TalentConfig.Icon.LoadAssetAsync();
            m_TalentToggleImage.sprite = await handle;
            Addressables.Release(handle);
            
            var playerData = PhantomSystem.Instance.PlayerData;
            if (m_TalentConfig.PreTalent != null && !playerData.UnlockTalents.Contains(m_TalentConfig.PreTalent.Guid))
            {
                //如果前置天赋没有解锁则自己也不能解锁
                m_TalentToggleToggle.interactable = false;
                m_LockRectTransform.gameObject.SetActive(true);
            }
            else
            {
                m_TalentToggleToggle.interactable = true;
                m_LockRectTransform.gameObject.SetActive(false);
            }

            m_ActiveRectTransform.gameObject.SetActive(playerData.UnlockTalents.Contains(m_TalentConfig.Guid));
        }
    }
}