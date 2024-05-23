using Cysharp.Threading.Tasks;
using MyFramework.Runtime;
using PhantomDanmaku.Config;
using PhantomDanmaku.Runtime.System;
using PhantomDanmaku.Runtime.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace PhantomDanmaku.Runtime
{
    public class BattleComponent : GameFrameworkComponent
    {
        public static async UniTask StartBattle(int chapter,int level)
        {
            //打开加载界面
            await Components.UI.Open<LoadingUIForm>(null);
            
            SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
            
            //同步加载场景，需要下一帧才能查找改场景的对象。
            await UniTask.DelayFrame(1);
            
            
            //读取关卡配置
            var chapterDatabase = PhantomSystem.Get<ChapterDatabase>();
            var levelConfig = chapterDatabase.Values[chapter].LevelConfigs[level];
            
            //初始化地图
            //获取场景中的瓦片地图
            var grid = GameObject.Find("Grid").transform;
            var tilemapGround = grid.Find("Ground").GetComponent<Tilemap>();
            var tilemapObject = grid.Find("Object").GetComponent<Tilemap>();
            var tilemapWall = grid.Find("Wall").GetComponent<Tilemap>();
            var data = new MapGeneratorData(levelConfig, tilemapGround, tilemapWall,tilemapObject );
            //初始化地图生成器
            await MapGenerator.Instance.Init(data);
            //生成地图
            MapGenerator.Instance.GeneratorMap();
            
            Components.UI.Close<LoadingUIForm>();
            
            //加载完毕，显示Hud
            await Components.UI.Open<HUDUIForm>(Player.Instance);
            
            //场景加载完成后，执行关卡开始事件
            levelConfig.OnLevelStart.StartEvent().Forget();
        }
    }
}