using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace PhantomDanmaku
{
    public class GameUIForm : UIFormLogic
    {
        public TextMeshProUGUI HpTMP;
        public Image HpImage;
        public TextMeshProUGUI ShieldTMP;
        public Image ShieldImage;
        public TextMeshProUGUI EnergyTMP;
        public Image EnergyImage;


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            //监听玩家生成事件，来初始化游戏面板，所以这个脚本应该在玩家生成之前执行
            GameSystem.Instance.AddEventListener<EntityBase>(CustomEvent.PlayerSpawn, Refresh);
            GameSystem.Instance.AddEventListener<EntityBase>(CustomEvent.PlayerWounded, Refresh);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            //取消监听
            GameSystem.Instance.RemoveEventListener<EntityBase>(CustomEvent.PlayerSpawn, Refresh);
            GameSystem.Instance.RemoveEventListener<EntityBase>(CustomEvent.PlayerWounded, Refresh);
        }

        public void Refresh(EntityBase player)
        {
            UpdateBar(HpTMP, HpImage, player.CurHp, player.Hp);
            UpdateBar(ShieldTMP, ShieldImage, player.CurShield, player.Shield);
            UpdateBar(EnergyTMP, EnergyImage, player.CurEnergy, player.Energy);
        }

        void UpdateBar(TextMeshProUGUI textMeshProUGUI, Image image, int curValue, int oriValue)
        {
            textMeshProUGUI.text = $"{curValue}/{oriValue}";
            image.fillAmount = (float)curValue / oriValue;
        }
    }
}