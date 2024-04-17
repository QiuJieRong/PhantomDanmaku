using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

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
        public Dictionary<RoomType, List<AssetReferenceGameObject>> RoomPrefabDic;

        [LabelText("房间数量配置")]
        public Dictionary<RoomType, Vector2> RoomCountDic;
        
        [LabelText("房间间距")]
        public int RoomDistance;

        [LabelText("道路宽度")]
        public int RoadWidth;

        [Title("关卡开始事件")]
        public Event OnLevelStart;
        
        [Title("关卡结束事件")]
        public Event OnLevelEnd;
    }
}