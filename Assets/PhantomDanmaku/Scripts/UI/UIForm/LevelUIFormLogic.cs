using Cysharp.Threading.Tasks;
using PhantomDanmaku.Config;
using PhantomDanmaku.Runtime.System;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace PhantomDanmaku.Runtime.UI
{
    public partial class LevelUIForm
    {
        private int m_SelectedChapterIdx;

        private int m_SelectedLevelIdx;

        private ChapterDatabase m_ChapterDatabase;
        
        private ChapterConfig SelectedChapterConfig => m_ChapterDatabase.Values[m_SelectedChapterIdx];
        
        private LevelConfig SelectedLevelConfig => SelectedChapterConfig.LevelConfigs[m_SelectedLevelIdx];
        
        public override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_ChapterDatabase = PhantomSystem.Get<ChapterDatabase>();
            //前一章
            m_PreChapterBtnButton.onClick.AddListener(() =>
            {
                if (m_SelectedChapterIdx - 1 >= 0)
                {
                    --m_SelectedChapterIdx;
                    Refresh(null);
                }
            });
            
            //下一章
            m_NextChapterBtnButton.onClick.AddListener(() =>
            {
                var maxChapterIdx = m_ChapterDatabase.Values.Count - 1;
                //如果存在下一章的话
                if (m_SelectedChapterIdx + 1 <= maxChapterIdx)
                {
                    ++m_SelectedChapterIdx;
                    m_SelectedLevelIdx = 0;
                    Refresh(null);
                }
            });
            
            //关闭
            m_CloseBtnButton.onClick.AddListener(() =>
            {
                Components.UI.Close(this);
            });
            
            //开始关卡
            m_StartBtnButton.onClick.AddListener(async () =>
            {
                //卸载大厅场景，关闭选关界面
                Components.UI.Close(this);
                await SceneManager.UnloadSceneAsync("HallScene");
                Components.Battle.StartBattle(m_SelectedChapterIdx, m_SelectedLevelIdx).Forget();
            });
            
            RegisterUIMessage("RefreshLevelUIForm",Refresh);
            RegisterUIMessage("SelectLevel",SelectLevel);
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Refresh(null);
        }

        private async void Refresh(object userData)
        {
            //根据章节更新标题
            m_ChapterTitleTextMeshProUGUI.text = $"第{m_SelectedChapterIdx + 1}章";
            
            //根据章节刷新关卡列表
            var idx = 0;
            foreach (var levelConfig in SelectedChapterConfig.LevelConfigs)
            {
                if (idx < m_ContentRectTransform.childCount)
                {
                    var go = m_ContentRectTransform.GetChild(idx).gameObject;
                    go.SetActive(true);
                    var levelBtn = go.GetUIGroup<LevelBtn>();
                    levelBtn.OnInit((m_SelectedChapterIdx,idx));
                }
                else
                {
                    //实例化新的按钮对象
                    var handle = Addressables.InstantiateAsync("Assets/PhantomDanmaku/Prefabs/UI/LevelBtn.prefab",m_ContentRectTransform);
                    AddDependence(handle);
                    var go = await handle;
                    var levelBtn = go.AddUIGroup<LevelBtn>((m_SelectedChapterIdx,idx));
                }
                ++idx;
            }

            while (idx < m_ContentRectTransform.childCount)
            {
                m_ContentRectTransform.GetChild(idx).gameObject.SetActive(false);
                ++idx;
            }
            
            //根据当前章节禁用下一章、上一章按钮,如果下一章没有解锁则不能点击
            m_PreChapterBtnButton.interactable = m_SelectedChapterIdx > 0;
            m_NextChapterBtnButton.interactable = m_SelectedChapterIdx + 1 < m_ChapterDatabase.Values.Count && PhantomSystem.Instance.PlayerData.ChapterIsUnlock(m_SelectedChapterIdx + 1);
            
            //根据当前选择关卡刷新标题和介绍
            m_LevelNameTextMeshProUGUI.text = SelectedLevelConfig.Name;
            m_LevelDescTextMeshProUGUI.text = SelectedLevelConfig.Desc;
        }

        private void SelectLevel(object userData)
        {
            if (userData is int levelIdx)
            {
                m_SelectedLevelIdx = levelIdx;
                Refresh(null);
            }
        }
    }
}