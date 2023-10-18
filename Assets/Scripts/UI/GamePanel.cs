using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : PanelBase
{
    public override void Show()
    {
        base.Show();
        //监听玩家生成事件，来初始化游戏面板，所以这个脚本应该在玩家生成之前执行
        EventCenter.Instance.AddEventListener<EntityBase>(CustomEvent.PlayerSpawn, UpdatePanel);
        EventCenter.Instance.AddEventListener<EntityBase>(CustomEvent.PlayerWounded, UpdatePanel);
    }
    public override void Hide()
    {
        base.Hide();
        //取消监听
        EventCenter.Instance.RemoveEventListener<EntityBase>(CustomEvent.PlayerSpawn, UpdatePanel);
        EventCenter.Instance.RemoveEventListener<EntityBase>(CustomEvent.PlayerWounded, UpdatePanel);
    }
    public void UpdatePanel(EntityBase player)
    {
        UpdateBar("Text_Hp","Image_HpBar",player.Hp,player.CurHp);
        UpdateBar("Text_Shield","Image_ShieldBar",player.Shield,player.CurShield);
        UpdateBar("Text_Energy","Image_EnergyBar",player.Energy,player.CurEnergy);
    }
    void UpdateBar(string textName, string imageName,int initData,int curData)
    {
        GetControl<Text>(textName).text = $"{curData}/{initData}";
        Image HpBar = GetControl<Image>(imageName);
        HpBar.rectTransform.localScale = new Vector3((float)curData / initData, 1, 1);
    }
}
