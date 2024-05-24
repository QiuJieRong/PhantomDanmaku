using Cysharp.Threading.Tasks;
using PhantomDanmaku.Config;
using PhantomDanmaku.Runtime.System;
using UnityEngine.SceneManagement;

namespace PhantomDanmaku.Runtime.UI
{
    public partial class LevelEndUIForm
    {
        private int m_CurChapterIdx;
        private int m_CurLevelIdx;
        public override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_AgainBtnButton.onClick.AddListener(async () =>
            {
                Components.ObjectPool.Clear();
                Components.UI.Close(this);
                await SceneManager.UnloadSceneAsync("BattleScene");
                await Components.Battle.StartBattle(m_CurChapterIdx, m_CurLevelIdx);
            });
            
            m_NextLevelBtnButton.onClick.AddListener(async () =>
            {
                Components.ObjectPool.Clear();
                Components.UI.Close(this);
                await SceneManager.UnloadSceneAsync("BattleScene");
                ++m_CurLevelIdx;
                await Components.Battle.StartBattle(m_CurChapterIdx, m_CurLevelIdx);
            });
            
            m_BackHallBtnButton.onClick.AddListener(async () =>
            {
                //返回大厅
                Components.ObjectPool.Clear();
                Components.UI.Close(this);
                await SceneManager.UnloadSceneAsync("BattleScene");
                
                //进入大厅
                SceneManager.LoadScene("HallScene", LoadSceneMode.Additive);
            });
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            
            if (userData is (int curChapter, int curLevelIdx, bool clear))
            {
                m_CurChapterIdx = curChapter;
                m_CurLevelIdx = curLevelIdx;
                Refresh(clear);
            }
        }

        private void Refresh(object userData)
        {
            if (userData is bool clear)
            {
                var chapterDatabase = PhantomSystem.Get<ChapterDatabase>();
                var chapterConfig = chapterDatabase.Values[m_CurChapterIdx];
                var levelCount = chapterConfig.LevelConfigs.Count;
                //显隐重新按钮
                m_AgainBtnButton.gameObject.SetActive(!clear);
                //显隐下一关按钮
                m_NextLevelBtnButton.gameObject.SetActive(m_CurLevelIdx + 1 < levelCount && clear);
                
                m_MessageTextMeshProUGUI.text = clear ? "恭喜过关" : "挑战失败";
            }
        }
    }
}