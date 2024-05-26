using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using PhantomDanmaku.Config;
using PhantomDanmaku.Runtime.System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PhantomDanmaku.Runtime.UI
{
    public partial class TalentUIForm
    {
        /// <summary>
        /// 当前选中的天赋
        /// </summary>
        private TalentConfig m_TalentConfig;

        /// <summary>
        /// 根节点列表
        /// </summary>
        private List<TalentNode> m_TalentRootNodes;

        private List<TalentToggle> m_TalentToggles;
        
        public override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_TalentRootNodes = new List<TalentNode>();
            m_TalentToggles = new List<TalentToggle>();
            
            m_UpgradeBtnButton.onClick.AddListener(() =>
            {
                var playerData = PhantomSystem.Instance.PlayerData;
                //如果技能点足够则点亮该天赋
                if (m_TalentConfig.SkillPointCost <= playerData.SkillPoint)
                {
                    if (!playerData.UnlockTalents.Contains(m_TalentConfig.Guid))
                    {
                        playerData.UnlockTalents.Add(m_TalentConfig.Guid);
                        //结算天赋效果
                        m_TalentConfig.TalentEffect.Invoke(playerData);
                    }
                    else
                    {
                        Debug.Log("该天赋以升级");
                    }
                }
                Refresh(null);
                //保存
                PhantomSystem.Instance.SavePlayerData();
            });
            m_CloseBtnButton.onClick.AddListener(() =>
            {
                Components.UI.Close(this);
            });
            RegisterUIMessage("SelectTalent",SelectTalent);
            
            //读取配置，生成天赋树
            var database = PhantomSystem.Get<TalentDatabase>();

            var talentConfigs = database.Values.ToList();
            
            //找到所有的根节点
            foreach (var talentConfig in talentConfigs.Where(talentConfig => talentConfig.PreTalent == null))
            {
                var node = new TalentNode(talentConfig);
                m_TalentRootNodes.Add(node);
                GenerateTalentTree(node, talentConfigs);
            }
            
            //绘制所有根节点
            DrawTalentTree(m_TalentRootNodes[0],m_Root1RectTransform);
            DrawTalentTree(m_TalentRootNodes[1],m_Root2RectTransform);

        }

        /// <summary>
        /// 生成某个根节点的天赋树
        /// </summary>
        /// <param name="curNode"></param>
        private void GenerateTalentTree(TalentNode curNode,List<TalentConfig> configs)
        {
            //找到所有前置天赋是该节点配置的配置
            foreach (var talentConfig in configs.Where(config => config.PreTalent == curNode.TalentConfig).ToList())
            {
                var node = new TalentNode(talentConfig);
                //将其作为该节点的子节点
                curNode.Children.Add(node);
                //并继续为子节点查找子节点
                GenerateTalentTree(node,configs);
            }
        }

        /// <summary>
        /// 绘制天赋树，生成按钮
        /// </summary>
        private async void DrawTalentTree(TalentNode node,Transform parent)
        {
            //根据传入节点生成按钮
            var handle = Addressables.InstantiateAsync("Assets/PhantomDanmaku/Prefabs/UI/TalentToggle.prefab", parent);
            var go = await handle;
            var talentToggle = go.AddUIGroup<TalentToggle>(node.TalentConfig);
            m_TalentToggles.Add(talentToggle);
            //设置组
            talentToggle.Toggle.group = m_ContentToggleGroup;
            
            //再继续绘制子节点
            foreach (var talentNode in node.Children)
            {
                DrawTalentTree(talentNode, talentToggle.ChildrenTransform);
            }
        }

        private void SelectTalent(object userData)
        {
            if (userData is TalentConfig talentConfig)
            {
                m_TalentConfig = talentConfig;
                Refresh(null);
            }
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            Refresh(null);
        }

        private void Refresh(object userData)
        {
            if (m_TalentConfig != null)
            {
                //根据当前选中的天赋刷新名字和描述
                m_TalentNameTextMeshProUGUI.text = m_TalentConfig.TalentName;
                m_TalentDescTextMeshProUGUI.text = m_TalentConfig.TalentDesc;
            }
            else
            {
                //如果没有选中则默认选中第一个根节点
                SelectTalent(m_TalentRootNodes[0].TalentConfig);
            }

            //刷新所有的天赋按钮
            foreach (var talentToggle in m_TalentToggles)
            {
                talentToggle.Refresh();
            }
        }
    }

    public class TalentNode
    {
        private TalentConfig m_TalentConfig;

        public TalentConfig TalentConfig => m_TalentConfig;

        private List<TalentNode> m_Children;

        public List<TalentNode> Children => m_Children;

        public TalentNode(TalentConfig talentConfig)
        {
            m_Children = new List<TalentNode>();
            m_TalentConfig = talentConfig;
        }
    }
}