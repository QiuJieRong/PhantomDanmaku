using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
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
        [FormerlySerializedAs("width")] [SerializeField]
        private int m_Width;
        public int Width => m_Width;
        [FormerlySerializedAs("height")] [SerializeField]
        private int m_Height;
        public int Height => m_Height;
        [FormerlySerializedAs("type")] [Header("房间类型")] public RoomType m_RoomType;
        public TileBase tileBase_ground;
        public TileBase tileBase_wall;

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector3(0.5f, 0.5f, 0), new Vector3(m_Width, m_Height, 1));
        }
    }

}