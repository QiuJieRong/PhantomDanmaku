using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PhantomDanmaku
{
    
    public enum RoomType
    {
        FightRoom,
        FirstRoom,
        RewardRoom,
        FinalRoom
    }

    public class RoomInfo : MonoBehaviour
    {
        [SerializeField]
        private int width;
        public int Width => width;
        [SerializeField]
        private int height;
        public int Height => height;
        [Header("房间类型")] public RoomType type;
        public TileBase tileBase_ground;
        public TileBase tileBase_wall;

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector3(0.5f, 0.5f, 0), new Vector3(width, height, 1));
        }
    }

}