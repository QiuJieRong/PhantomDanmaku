using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PhantomDanmaku.Runtime.Behavior
{
    public class CanAttackPlayer : Conditional
    {
        private Tilemap m_WallTileMap;
        
        private MonsterBase m_Monster;

        /// <summary>
        /// 是否是近战
        /// </summary>
        private bool m_CloseIn;
        
        public override void OnStart()
        {
            base.OnStart();
            m_WallTileMap = MapGenerator.Instance.TilemapWall;
            //判断该怪物是近战还是远程
            m_Monster = GetComponent<MonsterBase>();
            m_CloseIn = m_Monster.CurWeapon is CloseInWeaponBase;
        }


        public override TaskStatus OnUpdate()
        {
            var monsterTransform = transform;

            if (m_Monster.CurRoom != Player.Instance.CurRoom)
            {
                return TaskStatus.Failure;
            }
            
            //如果是近战攻击
            if (m_CloseIn)
            {
                //判断距离足够就返回true
                return Vector3.Distance(monsterTransform.position, Player.Instance.transform.position) < 1f ? TaskStatus.Success : TaskStatus.Failure;
            }
            else
            {
                //发射一条射线。如果能够射到玩家则返回true，不能则返回false
                var dir = (Player.Instance.transform.position - transform.position).normalized;
                var hits = new List<RaycastHit2D>();
                Physics2D.Raycast(monsterTransform.position, dir, new ContactFilter2D(), hits);

                var wallOrPlayerHits = new List<RaycastHit2D>();
                
                foreach (var hit in hits)
                {
                    //如果是不同阵营，或者不是子弹和物品，则加入碰撞列表
                    if (
                        ((1 << hit.transform.gameObject.layer) & (1 << LayerMask.NameToLayer(m_Monster.Camp.ToString()))) == 0 &&
                        ((1 << hit.transform.gameObject.layer) & LayerMask.GetMask("Bullet")) == 0 &&
                        ((1 << hit.transform.gameObject.layer) & LayerMask.GetMask("Item")) == 0
                    )
                    {
                        wallOrPlayerHits.Add(hit);
                    }
                }

                //如果第一个碰撞是玩家，则说明可以攻击玩家
                if (wallOrPlayerHits.Count > 0 && wallOrPlayerHits[0].transform == Player.Instance.transform)
                {
                    return TaskStatus.Success;
                }

                return TaskStatus.Failure;
            }
        }
    }
}