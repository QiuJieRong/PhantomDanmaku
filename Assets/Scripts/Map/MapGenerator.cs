using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapGenerator : SingletonMono<MapGenerator>
{

    [Header("总生成房间数")] public int roomNumber = 5;
    [Header("生成房间类型列表")] public List<E_Room_Type> roomTypeList;
    [Header("最大房间宽")] public int roomMaxW = 25;
    [Header("最大房间高")] public int roomMaxH = 25;
    [Header("最小房间宽")] public int roomMinW = 15;
    [Header("最小房间高")] public int roomMinH = 15;
    [Header("房间的间隔距离")] public int distance = 30;
    [Header("道路的宽度")] public int roadWidth = 5;
    [Header("怪物密度")] public float monsterDensity = 1;
    [Header("地板")] public TileBase tileBase_ground;
    [Header("墙")] public TileBase tileBase_wall;
    [Header("门")] public TileBase tileBase_door;
    [Header("地图地板")] public Tilemap tilemap_ground;
    [Header("地图墙壁")] public Tilemap tilemap_wall;
    [Header("地图对象")] public Tilemap tilemap_object;
    private List<Vector2Int> roomMap = new();//用来标记是否存在房间的List
    public List<Vector2Int> RoomMap => roomMap;
    private List<Room> rooms = new();//存储生成的房间对象
    public List<Room> Rooms => rooms;
    Vector2Int point = new Vector2Int(0, 0);//当前要生成房间的坐标
    void Start()
    {
        UIMgr.Instance.ShowPanel<GamePanel>("GamePanel");
        GeneratorMap();
        EventCenter.Instance.AddEventListener<Room>(CustomEvent.RoomEnter, RoomEnterCallback);
        EventCenter.Instance.AddEventListener<Room>(CustomEvent.RoomClear, RoomClearCallback);
    }

    void RoomEnterCallback(Room room)
    {
        //如果该房间还未清空怪物，则关门
        if (!room.isClear)
        {
            room.CloseDoor();
        }
    }
    void RoomClearCallback(Room room)
    {
        room.isClear = true;
        room.OpenDoor();
    }

    /// <summary>
    /// 切换场景时，移除所有事件监听
    /// </summary>
    void OnDestroy()
    {
        EventCenter.Instance.Clear();
        UIMgr.Instance.HidePanel("GamePanel");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            PoolMgr.Instance.Clear();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            foreach (var room in rooms)
            {
                room.OpenDoor();
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            foreach (var room in rooms)
            {
                room.CloseDoor();
            }
        }
    }

    /// <summary>
    /// 生成地图
    /// </summary>
    void GeneratorMap()
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
        //存储要生成房间的坐标和房间
        GameObject tileRoom;
        int id = 0;
        foreach (var type in roomTypeList)
        {
            switch (type)
            {
                case E_Room_Type.FirstRoom:
                    tileRoom = Resources.Load<GameObject>("Prefabs/Rooms/FirstRooms/FirstRoom" + Random.Range(0, 2));
                    break;
                case E_Room_Type.FightRoom:
                    tileRoom = Resources.Load<GameObject>("Prefabs/Rooms/FightRooms/FightRoom" + Random.Range(0, 5));
                    break;
                default:
                    tileRoom = Resources.Load<GameObject>("Prefabs/Rooms/FightRooms/FightRoom" + Random.Range(0, 5));
                    break;
            }
            roomMap.Add(point);
            rooms.Add(new Room(point, id, tileRoom));
            ++id;
            ChangeGeneratorPoint();
        }
    }
    /// <summary>
    /// 根据生成的房间信息，来绘制房间,和道路
    /// </summary>
    void DrawRoom()
    {
        foreach (Room room in rooms)
        {
            //绘制地板
            room.DrawGround();
            //绘制墙壁
            room.DrawWall();
            //复制房间预设体中的对象瓦片
            room.CopyRoom();
        }
        //道路绘制需要房间绘制完毕后绘制，否则无法正常的消除房间自己生成的墙壁
        foreach (Room room in rooms)
        {
            //判断右侧和上侧是否有房间，有的话就生成道路
            room.DrawRoad(roadWidth);
        }
    }

    /// <summary>
    /// 改变下一个生成点位
    /// </summary>
    void ChangeGeneratorPoint()
    {
        int dir = Random.Range(0, 4);//0:上 1:下 2:右 3:左 
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
        }
        while (roomMap.Contains(point));
    }
}
