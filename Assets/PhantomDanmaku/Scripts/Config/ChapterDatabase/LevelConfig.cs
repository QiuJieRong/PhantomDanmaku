using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;

namespace PhantomDanmaku.Config
{
    public class LevelConfig : SerializedConfig
    {
        [LabelText("关卡Id")]
        public int Id;

        [LabelText("关卡名字")]
        public string Name;
        
        [LabelText("关卡描述")]
        public string Desc;

        [LabelText("房间配置")]
        public Dictionary<RoomType, RoomPrefabList> RoomPrefabDic;

        [LabelText("房间数量配置")]
        public Dictionary<RoomType, Vector2Int> RoomCountDic;
        
        [LabelText("房间间距")]
        public int RoomDistance;

        [LabelText("道路宽度")]
        public int RoadWidth;

        [LabelText("地板")]
        public TileBase TileBaseGround;

        [LabelText("墙")]
        public TileBase TileBaseWall;

        [LabelText("门")]
        public TileBase TileBaseDoor;
        
        [Title("关卡开始事件")]
        public Event OnLevelStart;
        
        [Title("关卡结束事件")]
        public Event OnLevelEnd;
    }

    [LabelText("房间预设列表")]
    public class RoomPrefabList
    {
        [DrawWithUnity]
        public List<AssetReferenceGameObject> RoomPrefabs;
    }
}