using System;
using System.Collections.Generic;
using PhantomDanmaku.Config;
using PhantomDanmaku.Runtime.System;

namespace PhantomDanmaku.Runtime
{
    [Serializable]
    public class PlayerData
    {
        /// <summary>
        /// 通关最高的章节
        /// </summary>
        private int m_MaxChapterIdx;

        /// <summary>
        /// 通关了最高章节的第几个关卡
        /// </summary>
        private int m_MaxLevelIdx;

        /// <summary>
        /// 玩家等级
        /// </summary>
        private int m_PlayerLevel;

        /// <summary>
        /// 积累了多少经验
        /// </summary>
        private int m_Exp;

        /// <summary>
        /// 剩余技能点
        /// </summary>
        private int m_SkillPoint;

        public int SkillPoint => m_SkillPoint;

        #region 战斗相关

        /// <summary>
        /// 玩家属性字典
        /// </summary>
        private Dictionary<State,StateValue> m_StateDic;

        #endregion

        /// <summary>
        /// 解锁的天赋Guid列表
        /// </summary>
        private List<string> m_UnlockTalents;

        public List<string> UnlockTalents => m_UnlockTalents;

        public PlayerData()
        {
            m_MaxChapterIdx = 0;
            m_MaxLevelIdx = -1;//-1代表一关都没通过
            m_PlayerLevel = 1;
            m_Exp = 0;
            m_SkillPoint = 0;
            m_StateDic = new Dictionary<State, StateValue>();
            m_UnlockTalents = new List<string>();
            
            //读取默认数据配置
            var database = PhantomSystem.Get<DefaultDatabase>();
            var defaultValueDic = database.Values[0].DefaultPlayerStateDic;
            foreach (var kvp in defaultValueDic)
            {
                m_StateDic.Add(kvp.Key,kvp.Value);
            }
        }

        /// <summary>
        /// 获得玩家属性
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public StateValue GetStateValue(State state)
        {
            return m_StateDic.TryGetValue(state, out var stateValue) ? stateValue : null;
        }

        /// <summary>
        /// 记录通过某一关
        /// </summary>
        /// <param name="chapterIdx"></param>
        /// <param name="levelIdx"></param>
        public void LevelClear(int chapterIdx, int levelIdx)
        {
            var chapterDatabase = PhantomSystem.Get<ChapterDatabase>();
            
            //如果通过的关卡是该章节的最后一关，则解锁下一个章节
            if (levelIdx == chapterDatabase.Values[chapterIdx].LevelConfigs.Count - 1
                && chapterIdx + 1 < chapterDatabase.Values.Count
                && chapterIdx + 1 > m_MaxChapterIdx)
            {
                m_MaxChapterIdx = chapterIdx + 1;
                m_MaxLevelIdx = 0;
            }

            if (chapterIdx == m_MaxChapterIdx && levelIdx > m_MaxLevelIdx)
            {
                m_MaxLevelIdx = levelIdx;
            }
        }

        public bool ChapterIsUnlock(int chapter)
        {
            return chapter <= m_MaxChapterIdx;
        }
        
        public bool LevelIsUnLock(int chapter,int level)
        {
            if (m_MaxChapterIdx < chapter || (m_MaxChapterIdx == chapter && m_MaxLevelIdx < level - 1))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}