using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum E_Door_Type
{
    Horizontal,
    Vertical
}

public class Door
{
    private Tilemap tilemap_object => MapGenerator.Instance.tilemap_object;
    private TileBase door => MapGenerator.Instance.tileBase_door;
    private Vector2Int centerCoord;
    private int width;
    private E_Door_Type type;
    public Door(Vector2Int centerCoord, int width, E_Door_Type type)
    {
        this.centerCoord = centerCoord;
        this.width = width;
        this.type = type;
        Init();
    }
    void Init()
    {
        //设置ruletile,每个瓦片上都有一个对象
        tilemap_object.SetTile((Vector3Int)centerCoord, door);
        tilemap_object.SetColliderType((Vector3Int)centerCoord, Tile.ColliderType.None);
        SetSortingOrder(tilemap_object, (Vector3Int)centerCoord, 1);
        switch (type)
        {
            case E_Door_Type.Horizontal:
                for (int i = 1; i <= width / 2; i++)
                {
                    tilemap_object.SetTile((Vector3Int)centerCoord + new Vector3Int(i, 0, 0), door);
                    tilemap_object.SetTile((Vector3Int)centerCoord + new Vector3Int(-i, 0, 0), door);
                    tilemap_object.SetColliderType((Vector3Int)centerCoord + new Vector3Int(i, 0, 0), Tile.ColliderType.None);
                    tilemap_object.SetColliderType((Vector3Int)centerCoord + new Vector3Int(-i, 0, 0), Tile.ColliderType.None);
                    SetSortingOrder(tilemap_object, (Vector3Int)centerCoord + new Vector3Int(i, 0, 0), 1);
                    SetSortingOrder(tilemap_object, (Vector3Int)centerCoord + new Vector3Int(-i, 0, 0), 1);
                }
                break;
            case E_Door_Type.Vertical:
                for (int i = 1; i <= width / 2; i++)
                {
                    tilemap_object.SetTile((Vector3Int)centerCoord + new Vector3Int(0, i, 0), door);
                    tilemap_object.SetTile((Vector3Int)centerCoord + new Vector3Int(0, -i, 0), door);
                    tilemap_object.SetColliderType((Vector3Int)centerCoord + new Vector3Int(0, i, 0), Tile.ColliderType.None);
                    tilemap_object.SetColliderType((Vector3Int)centerCoord + new Vector3Int(0, -i, 0), Tile.ColliderType.None);
                    SetSortingOrder(tilemap_object, (Vector3Int)centerCoord + new Vector3Int(0, i, 0), 1);
                    SetSortingOrder(tilemap_object, (Vector3Int)centerCoord + new Vector3Int(0, -i, 0), 1);
                }
                break;
        }
    }
    public void Open()
    {
        int tileObjLayer = 1;
        //设置格子的碰撞类型，并设置该格子上的对象的层级
        tilemap_object.SetColliderType((Vector3Int)centerCoord, Tile.ColliderType.None);
        SetSortingOrder(tilemap_object, (Vector3Int)centerCoord, tileObjLayer);
        //获取门的中点坐标上的对象,将动画的IsOpen设置为true
        GameObject tileObj = tilemap_object.GetInstantiatedObject((Vector3Int)centerCoord);
        tileObj.GetComponentInChildren<Animator>().SetBool("IsOpen", true);
        switch (type)
        {
            case E_Door_Type.Horizontal:
                for (int i = 1; i <= width / 2; i++)
                {
                    tilemap_object.SetColliderType((Vector3Int)centerCoord + new Vector3Int(i, 0, 0), Tile.ColliderType.None);
                    SetSortingOrder(tilemap_object, (Vector3Int)centerCoord + new Vector3Int(i, 0, 0), tileObjLayer);
                    tileObj = tilemap_object.GetInstantiatedObject((Vector3Int)centerCoord + new Vector3Int(i, 0, 0));
                    tileObj.GetComponentInChildren<Animator>().SetBool("IsOpen", true);

                    tilemap_object.SetColliderType((Vector3Int)centerCoord + new Vector3Int(-i, 0, 0), Tile.ColliderType.None);
                    SetSortingOrder(tilemap_object, (Vector3Int)centerCoord + new Vector3Int(-i, 0, 0), tileObjLayer);
                    tileObj = tilemap_object.GetInstantiatedObject((Vector3Int)centerCoord + new Vector3Int(-i, 0, 0));
                    tileObj.GetComponentInChildren<Animator>().SetBool("IsOpen", true);
                }
                break;
            case E_Door_Type.Vertical:
                for (int i = 1; i <= width / 2; i++)
                {
                    tilemap_object.SetColliderType((Vector3Int)centerCoord + new Vector3Int(0, i, 0), Tile.ColliderType.None);
                    SetSortingOrder(tilemap_object, (Vector3Int)centerCoord + new Vector3Int(0, i, 0), tileObjLayer);
                    tileObj = tilemap_object.GetInstantiatedObject((Vector3Int)centerCoord + new Vector3Int(0, i, 0));
                    tileObj.GetComponentInChildren<Animator>().SetBool("IsOpen", true);

                    tilemap_object.SetColliderType((Vector3Int)centerCoord + new Vector3Int(0, -i, 0), Tile.ColliderType.None);
                    SetSortingOrder(tilemap_object, (Vector3Int)centerCoord + new Vector3Int(0, -i, 0), tileObjLayer);
                    tileObj = tilemap_object.GetInstantiatedObject((Vector3Int)centerCoord + new Vector3Int(0, -i, 0));
                    tileObj.GetComponentInChildren<Animator>().SetBool("IsOpen", true);
                }
                break;
        }
    }
    public void Close()
    {
        int tileObjLayer = 2;
        //设置格子的碰撞类型，并设置该格子上的对象的层级
        tilemap_object.SetColliderType((Vector3Int)centerCoord, Tile.ColliderType.Grid);
        SetSortingOrder(tilemap_object, (Vector3Int)centerCoord, tileObjLayer);
        //获取门的中点坐标上的对象,将动画的IsOpen设置为true
        GameObject tileObj = tilemap_object.GetInstantiatedObject((Vector3Int)centerCoord);
        tileObj.GetComponentInChildren<Animator>().SetBool("IsOpen", false);
        switch (type)
        {
            case E_Door_Type.Horizontal:
                for (int i = 1; i <= width / 2; i++)
                {
                    tilemap_object.SetColliderType((Vector3Int)centerCoord + new Vector3Int(i, 0, 0), Tile.ColliderType.Grid);
                    SetSortingOrder(tilemap_object, (Vector3Int)centerCoord + new Vector3Int(i, 0, 0), tileObjLayer);
                    tileObj = tilemap_object.GetInstantiatedObject((Vector3Int)centerCoord + new Vector3Int(i, 0, 0));
                    tileObj.GetComponentInChildren<Animator>().SetBool("IsOpen", false);

                    tilemap_object.SetColliderType((Vector3Int)centerCoord + new Vector3Int(-i, 0, 0), Tile.ColliderType.Grid);
                    SetSortingOrder(tilemap_object, (Vector3Int)centerCoord + new Vector3Int(-i, 0, 0), tileObjLayer);
                    tileObj = tilemap_object.GetInstantiatedObject((Vector3Int)centerCoord + new Vector3Int(-i, 0, 0));
                    tileObj.GetComponentInChildren<Animator>().SetBool("IsOpen", false);
                }
                break;
            case E_Door_Type.Vertical:
                for (int i = 1; i <= width / 2; i++)
                {
                    tilemap_object.SetColliderType((Vector3Int)centerCoord + new Vector3Int(0, i, 0), Tile.ColliderType.Grid);
                    SetSortingOrder(tilemap_object, (Vector3Int)centerCoord + new Vector3Int(0, i, 0), tileObjLayer);
                    tileObj = tilemap_object.GetInstantiatedObject((Vector3Int)centerCoord + new Vector3Int(0, i, 0));
                    tileObj.GetComponentInChildren<Animator>().SetBool("IsOpen", false);

                    tilemap_object.SetColliderType((Vector3Int)centerCoord + new Vector3Int(0, -i, 0), Tile.ColliderType.Grid);
                    SetSortingOrder(tilemap_object, (Vector3Int)centerCoord + new Vector3Int(0, -i, 0), tileObjLayer);
                    tileObj = tilemap_object.GetInstantiatedObject((Vector3Int)centerCoord + new Vector3Int(0, -i, 0));
                    tileObj.GetComponentInChildren<Animator>().SetBool("IsOpen", false);
                }
                break;
        }
    }
    /// <summary>
    /// 设置瓦片实例化出来的对象的层级
    /// </summary>
    void SetSortingOrder(Tilemap tilemap, Vector3Int cellCoord, int layer)
    {
        GameObject tileObj = tilemap.GetInstantiatedObject(cellCoord);
        tileObj.GetComponentInChildren<SpriteRenderer>().sortingOrder = layer;
    }
}
