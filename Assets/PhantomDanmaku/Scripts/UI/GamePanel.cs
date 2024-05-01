using Cysharp.Threading.Tasks;
using PhantomDanmaku.Config;
using PhantomDanmaku.Scripts.System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace PhantomDanmaku
{
    
    public class GamePanel : PanelBase
    {
        public override void Show()
        {
            base.Show();
            //监听玩家生成事件，来初始化游戏面板，所以这个脚本应该在玩家生成之前执行
            EventCenter.Instance.AddEventListener<EntityBase>(CustomEvent.PlayerSpawn, UpdatePanel);
            EventCenter.Instance.AddEventListener<EntityBase>(CustomEvent.PlayerWounded, UpdatePanel);
            
            //读取第一关关卡配置
            var chapterDatabase = PhantomSystem.Get<ChapterDatabase>();
            var levelConfig = chapterDatabase.Values[0].LevelConfigs[0];
            levelConfig.OnLevelStart.StartEvent().Forget();
            //初始化地图
            //获取场景中的瓦片地图
            var grid = GameObject.Find("Grid").transform;
            var tilemapGround = grid.Find("Ground").GetComponent<Tilemap>();
            var tilemapObject = grid.Find("Object").GetComponent<Tilemap>();
            var tilemapWall = grid.Find("Wall").GetComponent<Tilemap>();
            var data = new MapGeneratorData(levelConfig, tilemapGround, tilemapWall,tilemapObject );
            //初始化地图生成器
            MapGenerator.Instance.Init(data).ContinueWith(() =>
            {
                //生成地图
                MapGenerator.Instance.GeneratorMap();
            });
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

}