using PhantomDanmaku.Runtime.System;

namespace PhantomDanmaku.Runtime.UI
{
    public partial class LevelBtn
    {
        private int m_LevelIdx;
        public override void OnInit(object userData)
        {
            base.OnInit(userData);
            if (userData is (int chapterIdx,int levelIdx))
            {
                m_LevelIdx = levelIdx;
                m_LevelBtnButton.onClick.AddListener(() =>
                {
                    SendUIMessage("SelectLevel", m_LevelIdx);
                });
                m_IndexTextMeshProUGUI.text = $"{m_LevelIdx + 1}";
                
                //判断当前关卡是否解锁了
                var isUnlock = PhantomSystem.Instance.PlayerData.LevelIsUnLock(chapterIdx, levelIdx);
                m_IndexTextMeshProUGUI.gameObject.SetActive(isUnlock);
                m_LockRectTransform.gameObject.SetActive(!isUnlock);
                m_LevelBtnButton.interactable = isUnlock;
            }
        }
    }
}