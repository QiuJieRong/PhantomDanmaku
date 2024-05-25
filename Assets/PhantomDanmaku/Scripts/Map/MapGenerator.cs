using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PhantomDanmaku.Config;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Tilemaps;

namespace PhantomDanmaku.Runtime
{
    public class MapGenerator : SingletonBase<MapGenerator>
    {
        /// <summary>
        /// 关卡配置
        /// </summary>
        private LevelConfig m_LevelConfig;
        /// <summary>
        /// 生成房间类型字典
        /// </summary>
        private Dictionary<RoomType, List<GameObject>> m_RoomTypeDic;
        
        /// <summary>
        /// 房间间距
        /// </summary>
        private int m_RoomDistance;

        public int RoomDistance => m_RoomDistance;
        
        /// <summary>
        /// 道路宽度
        /// </summary>
        private int m_RoadWidth = 5;
        
        private TileBase m_TileBaseGround;
        public TileBase TileBaseGround => m_TileBaseGround;
        private TileBase m_TileBaseWall;
        public TileBase TileBaseWall => m_TileBaseWall;
        private TileBase m_TileBaseDoor;
        public TileBase TileBaseDoor => m_TileBaseDoor;
        public Tilemap TilemapGround { get; private set; }
        public Tilemap TilemapWall{ get; private set; }
        public Tilemap TilemapObject{ get; private set; }
        
        private List<Vector2Int> m_RoomPosList; //用来标记是否存在房间的List
        public List<Vector2Int> RoomPosList => m_RoomPosList;
        private List<Room> m_Rooms; //存储生成的房间对象
        public List<Room> Rooms => m_Rooms;
        private Vector2Int point = Vector2Int.zero; //当前要生成房间的坐标

        private List<AsyncOperationHandle> m_Handles;

        public async UniTask Init(object userData)
        {
            if (userData is MapGeneratorData mapGeneratorData)
            {
                point = Vector2Int.zero;
                m_Handles = new List<AsyncOperationHandle>();
                m_RoomPosList = new List<Vector2Int>();
                m_Rooms = new List<Room>();
                //加载房间配置
                m_RoomTypeDic = new Dictionary<RoomType, List<GameObject>>();
                m_LevelConfig = mapGeneratorData.LevelConfig;
                foreach (var kvp in m_LevelConfig.RoomPrefabDic)
                {
                    m_RoomTypeDic.Add(kvp.Key, new List<GameObject>());
                    foreach (var roomPrefab in kvp.Value.RoomPrefabs)
                    {
                        if (roomPrefab.IsValid())
                        {
                            
                        }
                        else
                        {
                            var handle = roomPrefab.LoadAssetAsync();
                            await handle;
                            m_Handles.Add(handle);
                        }
                        m_RoomTypeDic[kvp.Key].Add((GameObject)roomPrefab.Asset); 
                    }
                }

                m_RoomDistance = m_LevelConfig.RoomDistance;
                m_RoadWidth = m_LevelConfig.RoadWidth;
                //加载瓦片配置
                m_TileBaseGround = m_LevelConfig.TileBaseGround;
                m_TileBaseWall = m_LevelConfig.TileBaseWall;
                m_TileBaseDoor = m_LevelConfig.TileBaseDoor;
                //设置瓦片地图
                TilemapGround = mapGeneratorData.TilemapGround;
                TilemapWall = mapGeneratorData.TilemapWall;
                TilemapObject = mapGeneratorData.TilemapObject;
                Components.EventCenter.RemoveEventListener<Room>(CustomEvent.RoomEnter, RoomEnterCallback);
                Components.EventCenter.RemoveEventListener<Room>(CustomEvent.RoomClear, RoomClearCallback);
                Components.EventCenter.AddEventListener<Room>(CustomEvent.RoomEnter, RoomEnterCallback);
                Components.EventCenter.AddEventListener<Room>(CustomEvent.RoomClear, RoomClearCallback);
            }
        }

        private void RoomEnterCallback(Room room)
        {
            //如果该房间还未清空怪物，则关门
            if (!room.isClear)
            {
                room.CloseDoor();
            }
        }

        private void RoomClearCallback(Room room)
        {
            room.isClear = true;
            room.OpenDoor();

            if (room.Info.m_RoomType == RoomType.FinalRoom)
            {
                Components.EventCenter.EventTrigger("FinalRoomClear");
            }
        }

        /// <summary>
        /// 生成地图
        /// </summary>
        public void GeneratorMap()
        {
            //随机生成房间,并存储起来
            GeneratorRoom();
            //绘制房间(地板和墙壁和延申的道路)
            DrawRoom();
            //复制房间预设体中的对象瓦片
        }

        /// <summary>
        /// 生成房间存入rooms和roomMap中
        /// </summary>
        void GeneratorRoom()
        {
            var roomTypeDic = new Dictionary<RoomType, int>();
            //根据配置生成房间列表
            foreach (var kvp in m_RoomTypeDic)
            {
                //随机数量
                var count = m_LevelConfig.RoomCountDic[kvp.Key].GetRandom();
                roomTypeDic.Add(kvp.Key, count);
            }

            var roomPrefabList = new List<GameObject>();
            GameObject finalRoomPrefab = null;
            GameObject firstRoomPrefab = null;
            foreach (var kvp in roomTypeDic)
            {
                var count = kvp.Value;
                while (count-- > 0)
                {
                    var tileRoom = m_RoomTypeDic[kvp.Key].GetRandom();
                    if (kvp.Key == RoomType.FinalRoom)
                        finalRoomPrefab = tileRoom;
                    else if (kvp.Key == RoomType.FirstRoom)
                        firstRoomPrefab = tileRoom;
                    else
                        roomPrefabList.Add(tileRoom);
                }
            }
            
            var id = 0;
            //先生成初始房间
            m_RoomPosList.Add(point);
            m_Rooms.Add(new Room(point, id, firstRoomPrefab));
            ++id;
            ChangeGeneratorPoint();
            
            //再生成战斗房间和奖励房间
            //打乱顺序，防止相同类型的房间总是相邻
            roomPrefabList.Shuffle();
            foreach (var roomPrefab in roomPrefabList)
            {
                m_RoomPosList.Add(point);
                m_Rooms.Add(new Room(point, id, roomPrefab));
                ++id;
                ChangeGeneratorPoint();
            }

            //最后生成终点房间
            //一般终点房间只有一个,在所有房间位置确定后再生成，防止距离起点过近
            if (finalRoomPrefab != null)
            {
                m_RoomPosList.Add(point);
                m_Rooms.Add(new Room(point, id, finalRoomPrefab));
                ++id;
                ChangeGeneratorPoint();
            }
        }

        /// <summary>
        /// 根据生成的房间信息，来绘制房间,和道路
        /// </summary>
        private void DrawRoom()
        {
            foreach (var room in m_Rooms)
            {
                //绘制地板
                room.DrawGround();
                //绘制墙壁
                room.DrawWall();
                //复制房间预设体中的对象瓦片
                room.CopyRoom();
            }

            //道路绘制需要房间绘制完毕后绘制，否则无法正常的消除房间自己生成的墙壁
            foreach (Room room in m_Rooms)
            {
                //判断右侧和上侧是否有房间，有的话就生成道路
                room.DrawRoad(m_RoadWidth);
            }
        }

        /// <summary>
        /// 改变下一个生成点位
        /// </summary>
        private void ChangeGeneratorPoint()
        {
            int dir = Random.Range(0, 4); //0:上 1:下 2:右 3:左 
            do
            {
                switch (dir)
                {
                    case 0:
                        point += new Vector2Int(0, 1);
                        break;
                    case 1:
                        point += new Vector2Int(0, -1);
                        break;
                    case 2:
                        point += new Vector2Int(1, 0);
                        break;
                    case 3:
                        point += new Vector2Int(-1, 0);
                        break;
                }
            } while (m_RoomPosList.Contains(point));
        }

        public void Clear()
        {
            foreach (var room in m_Rooms)
            {
                room.Release();
            }
            m_Rooms = null;
        }
    }

    public class MapGeneratorData
    {
        public LevelConfig LevelConfig { get; }

        public Tilemap TilemapGround { get; }
        public Tilemap TilemapWall { get; }
        public Tilemap TilemapObject { get; }

        public MapGeneratorData(LevelConfig levelConfig, Tilemap tilemapGround, Tilemap tilemapWall,
            Tilemap tilemapObject)
        {
            LevelConfig = levelConfig;
            TilemapGround = tilemapGround;
            TilemapWall = tilemapWall;
            TilemapObject = tilemapObject;
        }
    }
}