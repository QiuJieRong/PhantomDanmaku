using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapGenerator : SingletonMono<MapGenerator>
{

    [Header("总生成房间数")] public int roomNumber = 5;
    [Header("最大房间宽")] public int roomMaxW = 25;
    [Header("最大房间高")] public int roomMaxH = 25;
    [Header("最小房间宽")] public int roomMinW = 15;
    [Header("最小房间高")] public int roomMinH = 15;
    [Header("房间的间隔距离")] public int distance = 30;
    [Header("道路的宽度")] public int roadWidth = 5;
    [Header("怪物密度")] public float monsterDensity = 1;
    [Header("地板")] public TileBase ground;
    [Header("墙")] public TileBase wall;
    [Header("地图地板")] public Tilemap tilemap_ground;
    [Header("地图墙壁")] public Tilemap tilemap_wall;
    private List<Vector2Int> roomMap = new();//用来标记是否存在房间的List
    public List<Vector2Int> RoomMap => roomMap;
    private List<Room> rooms = new();//存储生成的房间对象
    public List<Room> Rooms => rooms;
    Vector2Int point = new Vector2Int(0, 0);//当前要生成房间的坐标
    void Start()
    {
        UIMgr.Instance.ShowPanel<GamePanel>("GamePanel");
        GeneratorMap();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            PoolMgr.Instance.Clear();
        }
    }

    void GeneratorMap()
    {
        //随机生成房间,并存储起来
        GeneratorRoom();
        //绘制房间(地板和墙壁和延申的道路)
        DrawRoom();
    }

    /// <summary>
    /// 随机获取一个min到max之间的奇数
    /// </summary>
    int GetOddNumber(int min, int max)
    {
        int x;
        do
        {
            x = Random.Range(min, max);
        }
        while (x % 2 == 0);
        return x;
    }

    void GeneratorRoom()
    {
        //存储要生成房间的坐标和房间
        for (int i = 0; i < roomNumber; i++)
        {
            roomMap.Add(point);
            int width = GetOddNumber(roomMinW, roomMaxW);
            int height = GetOddNumber(roomMinH, roomMaxH);
            rooms.Add(new Room(point, width, height, i));//_roomMap列表和_rooms列表一一对应
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
        }
        //道路绘制需要房间绘制完毕后绘制，否则无法正常的消除房间自己生成的墙壁
        foreach (Room room in rooms)
        {
            //判断右侧和上侧是否有房间，有的话就生成道路
            room.DrawRoad(roadWidth);
        }
    }


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
