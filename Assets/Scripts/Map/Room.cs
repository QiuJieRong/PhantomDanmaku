using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room
{
    public int id;//房间id
    public Vector2Int gridCoord;//房间的坐标，间隔为1
    public Vector2Int CenterCoord//房间中心坐标，间隔为distance
    {
        get
        {
            return gridCoord * MapGenerator.Instance.distance;
        }
    }
    private int width;//房间宽
    public int Width => width;
    private int height;//房间高
    public int Height => height;
    private float monsterDensity;//房间怪物密度

    //获得地图生成器的tilemap 和tilebase 和roomMap和rooms
    private Tilemap tilemap_ground => MapGenerator.Instance.tilemap_ground;
    private Tilemap tilemap_wall => MapGenerator.Instance.tilemap_wall;
    private TileBase ground => MapGenerator.Instance.ground;
    private TileBase wall => MapGenerator.Instance.wall;
    private List<Vector2Int> roomMap => MapGenerator.Instance.RoomMap;
    private List<Room> rooms => MapGenerator.Instance.Rooms;


    public Room(Vector2Int gridCoord, int width, int height, int id)
    {
        this.gridCoord = gridCoord;
        this.width = width;
        this.height = height;
        this.id = id;
    }
    public void DrawGround()
    {
        //在中心坐标处绘制房间的地板，长宽随机，不过随机的结果是奇数
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3Int drawPos = (Vector3Int)(CenterCoord + new Vector2Int(i, j) - new Vector2Int(width / 2, height / 2));
                tilemap_ground.SetTile(drawPos, ground);
                //不在边缘生成，不在第一个房间生成
                if (Random.Range(0.0f, 100.0f) < monsterDensity && i * j != 0 && i < width - 1 && j < height - 1 && id != 0)
                {
                    GeneratorMonster(drawPos);
                }
            }
        }
    }
    public void DrawWall()
    {
        //在地板外围绘制一圈墙壁
        //顶部
        for (int i = -1; i <= width; i++)
        {
            Vector3Int drawPos = (Vector3Int)(CenterCoord + new Vector2Int(i, height) - new Vector2Int(width / 2, height / 2));
            tilemap_wall.SetTile(drawPos, wall);
        }
        //底部
        for (int i = -1; i <= width; i++)
        {
            Vector3Int drawPos = (Vector3Int)(CenterCoord + new Vector2Int(i, -1) - new Vector2Int(width / 2, height / 2));
            tilemap_wall.SetTile(drawPos, wall);
        }
        //右侧
        for (int i = -1; i <= height; i++)
        {
            Vector3Int drawPos = (Vector3Int)(CenterCoord + new Vector2Int(width, i) - new Vector2Int(width / 2, height / 2));
            tilemap_wall.SetTile(drawPos, wall);
        }
        //左侧
        for (int i = -1; i <= height; i++)
        {
            Vector3Int drawPos = (Vector3Int)(CenterCoord + new Vector2Int(-1, i) - new Vector2Int(width / 2, height / 2));
            tilemap_wall.SetTile(drawPos, wall);
        }
    }

    public void DrawRoad()
    {
        //如果有房间就连接这两个房间
        Room targetRoom;
        int targetRoomIndex;
        //如果上方有房间
        if (roomMap.Contains(gridCoord + new Vector2Int(0, 1)))
        {
            targetRoomIndex = roomMap.IndexOf(gridCoord + new Vector2Int(0, 1));
            targetRoom = rooms[targetRoomIndex];
            //绘制连接这两个房间的道路
            //道路起点
            Vector2Int roadStart = CenterCoord + new Vector2Int(0, Height / 2 + 1);
            //道路终点
            Vector2Int roadEnd = targetRoom.CenterCoord - new Vector2Int(0, targetRoom.Height / 2);
            //当前绘制点
            Vector2Int curpoint = roadStart;
            //将原本的墙壁消除
            MapGenerator.Instance.tilemap_wall.SetTile((Vector3Int)curpoint + new Vector3Int(0, 0, 0), null);
            MapGenerator.Instance.tilemap_wall.SetTile((Vector3Int)curpoint + new Vector3Int(-1, 0, 0), null);
            MapGenerator.Instance.tilemap_wall.SetTile((Vector3Int)curpoint + new Vector3Int(1, 0, 0), null);
            //道路长度
            int roadLength = (int)Vector2Int.Distance(roadStart, roadEnd);


            for (int j = 0; j < roadLength; j++)
            {
                tilemap_ground.SetTile((Vector3Int)curpoint, ground);
                tilemap_ground.SetTile((Vector3Int)curpoint + new Vector3Int(-1, 0, 0), ground);
                tilemap_ground.SetTile((Vector3Int)curpoint + new Vector3Int(1, 0, 0), ground);
                tilemap_wall.SetTile((Vector3Int)curpoint + new Vector3Int(-2, 0, 0), wall);
                tilemap_wall.SetTile((Vector3Int)curpoint + new Vector3Int(2, 0, 0), wall);
                curpoint += new Vector2Int(0, 1);
            }
            //将终点房间的墙消除
            curpoint += new Vector2Int(0, -1);//y为-1是因为curpoint在前面的循环中加了1
            tilemap_wall.SetTile((Vector3Int)curpoint + new Vector3Int(0, 0, 0), null);
            tilemap_wall.SetTile((Vector3Int)curpoint + new Vector3Int(-1, 0, 0), null);
            tilemap_wall.SetTile((Vector3Int)curpoint + new Vector3Int(1, 0, 0), null);
        }
        //如果右方有房间
        if (roomMap.Contains(gridCoord + new Vector2Int(1, 0)))
        {
            targetRoomIndex = roomMap.IndexOf(gridCoord + new Vector2Int(1, 0));
            targetRoom = rooms[targetRoomIndex];
            //绘制连接这两个房间的道路
            //道路起点
            Vector2Int roadStart = CenterCoord + new Vector2Int(Width / 2 + 1, 0);
            //道路终点
            Vector2Int roadEnd = targetRoom.CenterCoord - new Vector2Int(targetRoom.Width / 2, 0);
            //当前绘制点
            Vector2Int curpoint = roadStart;
            //将原本的墙壁消除
            tilemap_wall.SetTile((Vector3Int)curpoint + new Vector3Int(0, 0, 0), null);
            tilemap_wall.SetTile((Vector3Int)curpoint + new Vector3Int(0, 1, 0), null);
            tilemap_wall.SetTile((Vector3Int)curpoint + new Vector3Int(0, -1, 0), null);
            //道路长度
            int roadLength = (int)Vector2Int.Distance(roadStart, roadEnd);
            for (int j = 0; j < roadLength; j++)
            {
                tilemap_ground.SetTile((Vector3Int)curpoint, ground);
                tilemap_ground.SetTile((Vector3Int)curpoint + new Vector3Int(0, -1, 0), ground);
                tilemap_ground.SetTile((Vector3Int)curpoint + new Vector3Int(0, 1, 0), ground);
                tilemap_wall.SetTile((Vector3Int)curpoint + new Vector3Int(0, -2, 0), wall);
                tilemap_wall.SetTile((Vector3Int)curpoint + new Vector3Int(0, 2, 0), wall);
                curpoint += new Vector2Int(1, 0);
            }
            curpoint += new Vector2Int(-1, 0);//x为-1是因为curpoint在前面的循环中加了1
            //将目标房间的墙壁消除
            tilemap_wall.SetTile((Vector3Int)curpoint + new Vector3Int(0, 0, 0), null);
            tilemap_wall.SetTile((Vector3Int)curpoint + new Vector3Int(0, 1, 0), null);
            tilemap_wall.SetTile((Vector3Int)curpoint + new Vector3Int(0, -1, 0), null);
        }
    }

    void GeneratorMonster(Vector3 spawnPos)
    {
        GameObject monster = PoolMgr.Instance.GetObj("Prefabs/Entities/Monster");
        monster.transform.position = spawnPos;
    }
}
