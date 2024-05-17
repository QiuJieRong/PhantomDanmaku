using Cysharp.Threading.Tasks;
using MyFramework.Runtime;
using PhantomDanmaku.Config;
using PhantomDanmaku.Runtime.System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace PhantomDanmaku.Runtime.UI
{
    public class BattleComponent : GameFrameworkComponent
    {
        public async UniTask StartBattle(int chapter,int level)
        {
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Additive);

            await UniTask.DelayFrame(1);
            //读取关卡配置
            var chapterDatabase = PhantomSystem.Get<ChapterDatabase>();
            var levelConfig = chapterDatabase.Values[chapter].LevelConfigs[level];
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
                //发送地图生成结束通知
                GameEntry.EventCenter.EventTrigger("MapGeneratorComplete");
            });

            var sceneLoadCompletion = false;

            void LoadComplete()
            {
                sceneLoadCompletion = true;
                GameEntry.EventCenter.RemoveEventListener("MapGeneratorComplete",LoadComplete);
            }
                
            GameEntry.EventCenter.AddEventListener("MapGeneratorComplete",LoadComplete);

            await UniTask.WaitUntil(() => sceneLoadCompletion);
        }
    }
}