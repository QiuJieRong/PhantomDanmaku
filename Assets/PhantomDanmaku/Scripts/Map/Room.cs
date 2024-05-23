using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PhantomDanmaku.Runtime
{

    public class Room
    {
        public int id; //房间id
        public Vector2Int gridCoord; //房间的坐标，间隔为1

        public Vector2Int CenterCoord => gridCoord * MapGenerator.Instance.RoomDistance; //房间中心坐标，间隔为distance

        private GameObject roomPrefab; //Grid\Tilemap层级结构的预制件，Grid上挂载了RoomInfo组件

        public RoomInfo Info
        {
            get { return roomPrefab.GetComponent<RoomInfo>(); }
        }

        public bool isClear = true; //该房间是否清空怪物
        private List<GameObject> monsters = new(); //该房间存在的怪物列表

        private List<Door> doors = new();

        //获得地图生成器的tilemap  和roomMap和rooms等成员
        private static Tilemap TilemapGround => MapGenerator.Instance.TilemapGround;
        private static Tilemap TilemapWall => MapGenerator.Instance.TilemapWall;
        private static Tilemap TilemapObject => MapGenerator.Instance.TilemapObject;
        private static List<Vector2Int> RoomMap => MapGenerator.Instance.RoomPosList;
        private static List<Room> Rooms => MapGenerator.Instance.Rooms;


        public Room(Vector2Int gridCoord, int id, GameObject roomPrefab)
        {
            this.gridCoord = gridCoord;
            this.id = id;
            this.roomPrefab = roomPrefab;
            if (Info.m_RoomType == RoomType.FightRoom)
                isClear = false;
            Components.EventCenter.AddEventListener<GameObject>(CustomEvent.MonsterDead, MonsterDeadCallback);
        }

        void MonsterDeadCallback(GameObject monster)
        {
            if (monsters.Contains(monster))
            {
                monsters.Remove(monster);
            }

            if (monsters.Count == 0 && !isClear)
            {
                isClear = true;
                Components.EventCenter.EventTrigger(CustomEvent.RoomClear, this);
            }
        }

        public void DrawGround()
        {
            //在中心坐标处绘制房间的地板，长宽随机，不过随机的结果是奇数
            for (int i = 0; i < Info.Width; i++)
            {
                for (int j = 0; j < Info.Height; j++)
                {
                    Vector3Int drawPos = (Vector3Int)(CenterCoord + new Vector2Int(i, j) -
                                                      new Vector2Int(Info.Width / 2, Info.Height / 2));
                    TilemapGround.SetTile(drawPos, Info.tileBase_ground);
                }
            }
        }

        public void DrawWall()
        {
            //在地板外围绘制一圈墙壁
            //顶部
            for (int i = -1; i <= Info.Width; i++)
            {
                Vector3Int drawPos = (Vector3Int)(CenterCoord + new Vector2Int(i, Info.Height) -
                                                  new Vector2Int(Info.Width / 2, Info.Height / 2));
                TilemapWall.SetTile(drawPos, Info.tileBase_wall);
            }

            //底部
            for (int i = -1; i <= Info.Width; i++)
            {
                Vector3Int drawPos = (Vector3Int)(CenterCoord + new Vector2Int(i, -1) -
                                                  new Vector2Int(Info.Width / 2, Info.Height / 2));
                TilemapWall.SetTile(drawPos, Info.tileBase_wall);
            }

            //右侧
            for (int i = -1; i <= Info.Height; i++)
            {
                Vector3Int drawPos = (Vector3Int)(CenterCoord + new Vector2Int(Info.Width, i) -
                                                  new Vector2Int(Info.Width / 2, Info.Height / 2));
                TilemapWall.SetTile(drawPos, Info.tileBase_wall);
            }

            //左侧
            for (int i = -1; i <= Info.Height; i++)
            {
                Vector3Int drawPos = (Vector3Int)(CenterCoord + new Vector2Int(-1, i) -
                                                  new Vector2Int(Info.Width / 2, Info.Height / 2));
                TilemapWall.SetTile(drawPos, Info.tileBase_wall);
            }
        }

        public void DrawRoad(int roadWidth)
        {
            //如果有房间就连接这两个房间
            Room targetRoom;
            int targetRoomIndex;
            //如果上方有房间
            if (RoomMap.Contains(gridCoord + new Vector2Int(0, 1)))
            {
                targetRoomIndex = RoomMap.IndexOf(gridCoord + new Vector2Int(0, 1));
                targetRoom = Rooms[targetRoomIndex];
                //绘制连接这两个房间的道路
                //道路起点
                Vector2Int roadStart = CenterCoord + new Vector2Int(0, Info.Height / 2 + 1);
                //道路终点
                Vector2Int roadEnd = targetRoom.CenterCoord + new Vector2Int(0, -targetRoom.Info.Height / 2 - 1);
                //当前绘制点
                Vector2Int curpoint = roadStart;

                //清除起点和终点的墙壁并添加门
                for (int i = 0; i <= roadWidth / 2; i++)
                {
                    if (i == 0)
                    {
                        TilemapWall.SetTile((Vector3Int)roadStart, null); //起点
                        TilemapWall.SetTile((Vector3Int)roadEnd, null); //终点
                        doors.Add(new Door(roadStart, roadWidth, E_Door_Type.Horizontal)); //起点
                        targetRoom.doors.Add(new Door(roadEnd, roadWidth, E_Door_Type.Horizontal)); //终点
                    }
                    else
                    {
                        //起点
                        TilemapWall.SetTile((Vector3Int)roadStart + new Vector3Int(-i, 0, 0), null);
                        TilemapWall.SetTile((Vector3Int)roadStart + new Vector3Int(i, 0, 0), null);
                        //终点
                        TilemapWall.SetTile((Vector3Int)roadEnd + new Vector3Int(-i, 0, 0), null);
                        TilemapWall.SetTile((Vector3Int)roadEnd + new Vector3Int(i, 0, 0), null);
                    }
                }

                //道路长度
                int roadLength = (int)Vector2Int.Distance(roadStart, roadEnd);
                for (int i = 0; i <= roadLength; i++)
                {
                    //绘制道路
                    for (int j = 0; j <= roadWidth / 2; j++)
                    {
                        if (j == 0)
                        {
                            TilemapGround.SetTile((Vector3Int)curpoint, Info.tileBase_ground);
                        }
                        else
                        {
                            TilemapGround.SetTile((Vector3Int)curpoint + new Vector3Int(-j, 0, 0),
                                Info.tileBase_ground);
                            TilemapGround.SetTile((Vector3Int)curpoint + new Vector3Int(j, 0, 0),
                                Info.tileBase_ground);
                        }
                    }

                    //绘制道路的墙壁
                    TilemapWall.SetTile((Vector3Int)curpoint + new Vector3Int(roadWidth / 2 + 1, 0, 0),
                        Info.tileBase_wall);
                    TilemapWall.SetTile((Vector3Int)curpoint + new Vector3Int(-roadWidth / 2 - 1, 0, 0),
                        Info.tileBase_wall);
                    curpoint += new Vector2Int(0, 1);
                }
            }

            //如果右方有房间
            if (RoomMap.Contains(gridCoord + new Vector2Int(1, 0)))
            {
                targetRoomIndex = RoomMap.IndexOf(gridCoord + new Vector2Int(1, 0));
                targetRoom = Rooms[targetRoomIndex];
                //绘制连接这两个房间的道路
                //道路起点
                Vector2Int roadStart = CenterCoord + new Vector2Int(Info.Width / 2 + 1, 0);
                //道路终点
                Vector2Int roadEnd = targetRoom.CenterCoord + new Vector2Int(-targetRoom.Info.Width / 2 - 1, 0);
                //当前绘制点
                Vector2Int curpoint = roadStart;
                //将起点和终点的墙壁消除
                for (int i = 0; i <= roadWidth / 2; i++)
                {
                    if (i == 0)
                    {
                        TilemapWall.SetTile((Vector3Int)roadStart, null); //起点
                        TilemapWall.SetTile((Vector3Int)roadEnd, null); //终点
                        doors.Add(new Door(roadStart, roadWidth, E_Door_Type.Vertical)); //起点
                        targetRoom.doors.Add(new Door(roadEnd, roadWidth, E_Door_Type.Vertical)); //终点
                    }
                    else
                    {
                        //起点
                        TilemapWall.SetTile((Vector3Int)roadStart + new Vector3Int(0, -i, 0), null);
                        TilemapWall.SetTile((Vector3Int)roadStart + new Vector3Int(0, i, 0), null);
                        //终点
                        TilemapWall.SetTile((Vector3Int)roadEnd + new Vector3Int(0, -i, 0), null);
                        TilemapWall.SetTile((Vector3Int)roadEnd + new Vector3Int(0, i, 0), null);
                    }
                }

                //道路长度
                int roadLength = (int)Vector2Int.Distance(roadStart, roadEnd);
                for (int i = 0; i <= roadLength; i++)
                {
                    for (int j = 0; j <= roadWidth / 2; j++)
                    {
                        if (j == 0)
                        {
                            TilemapGround.SetTile((Vector3Int)curpoint, Info.tileBase_ground);
                        }
                        else
                        {
                            TilemapGround.SetTile((Vector3Int)curpoint + new Vector3Int(0, -j, 0),
                                Info.tileBase_ground);
                            TilemapGround.SetTile((Vector3Int)curpoint + new Vector3Int(0, j, 0),
                                Info.tileBase_ground);
                        }
                    }

                    //绘制道路两边的墙壁
                    TilemapWall.SetTile((Vector3Int)curpoint + new Vector3Int(0, roadWidth / 2 + 1, 0),
                        Info.tileBase_wall);
                    TilemapWall.SetTile((Vector3Int)curpoint + new Vector3Int(0, -roadWidth / 2 - 1, 0),
                        Info.tileBase_wall);
                    curpoint += new Vector2Int(1, 0);
                }
            }
        }

        public void CopyRoom()
        {
            //被复制的tilemap
            var targetTilemapWall = roomPrefab.transform.Find("Wall").GetComponent<Tilemap>() ?? throw new ArgumentNullException("roomPrefab.transform.Find(\"Wall\").GetComponent<Tilemap>()");
            var targetTilemapObject = roomPrefab.transform.Find("Object").GetComponent<Tilemap>();
            //获取要复制的所有tile
            foreach (var position in targetTilemapWall.cellBounds.allPositionsWithin)
            {
                if (targetTilemapWall.GetTile(position) != null)
                    TilemapWall.SetTile((Vector3Int)CenterCoord + position, targetTilemapWall.GetTile(position));
                if (targetTilemapObject.GetTile(position) != null)
                {
                    TilemapObject.SetTile((Vector3Int)CenterCoord + position, targetTilemapObject.GetTile(position));
                    GameObject obj = TilemapObject.GetInstantiatedObject((Vector3Int)CenterCoord + position);
                    monsters.Add(obj);
                }
            }

        }

        /// <summary>
        /// 开门
        /// </summary>
        public void OpenDoor()
        {
            foreach (Door door in doors)
            {
                door.Open();
            }
        }

        /// <summary>
        /// 关门
        /// </summary>
        public void CloseDoor()
        {
            foreach (Door door in doors)
            {
                door.Close();
            }
        }
    }

}