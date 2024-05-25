using Cysharp.Threading.Tasks;
using PhantomDanmaku.Runtime.UI;

namespace PhantomDanmaku.Runtime
{
    public class PortalExit : PortalBase
    {
        public override bool CanInteract => m_CanInteract;

        protected override void Start()
        {
            base.Start();
            m_CanInteract = false;
            //只有清空终点房间的怪物才可以交互
            //注册清空终点房间怪物事件监听
            Components.EventCenter.AddEventListener("FinalRoomClear",FinalRoomClearListener);
        }

        private void FinalRoomClearListener()
        {
            m_CanInteract = true;
        }

        private bool m_CanInteract;


        public override void Interact()
        {
            //打开关卡结束界面。传入通关
            Components.UI.Open<LevelEndUIForm>((Components.Battle.CurChapterIdx, Components.Battle.CurLevelIdx, true)).Forget();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Components.EventCenter.RemoveEventListener("FinalRoomClear",FinalRoomClearListener);
        }
    }
}