using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace PhantomDanmaku.Runtime.Behavior
{
    public class MoveToAttackPoint : Action
    {
        private MonsterBase m_Monster;

        private Stack<Vector2Int> m_Path;

        private Vector2Int m_CurPoint;

        /// <summary>
        /// 移动持续时间，如果时间到了还没到达目的地，则直接退出该行为
        /// </summary>
        private float m_DurationTime;

        public override async void OnStart()
        {
            base.OnStart();
            m_DurationTime = 2f;
            m_Monster = GetComponent<MonsterBase>();
            
            var pathFinder = new PathFinder();
            pathFinder.Init(MapGenerator.Instance.TilemapWall, m_Monster.V2IPos, Player.Instance.V2IPos);
            m_Path = await pathFinder.GetPathList();
            if (m_Path != null)
                m_CurPoint = m_Path.Pop();
        }

        public override TaskStatus OnUpdate()
        {
            m_DurationTime -= Time.deltaTime;
            if (m_DurationTime <= 0)
            {
                return TaskStatus.Failure;
            }
            
            if (m_Monster.CurHp <= 0)
            {
                return TaskStatus.Failure;
            }
            
            //如果没有到达当前点位，则继续前进
            if (Vector2.Distance(m_Monster.V2Pos, m_CurPoint) > 0.1)
            {
                //当未到达时，继续超方向前进
                var dir = (m_CurPoint - m_Monster.V2Pos).normalized;
                m_Monster.Rig2D.velocity = dir * m_Monster.Speed;
            }
            else if(m_Path is { Count: > 0 })
            {
                m_CurPoint = m_Path.Pop();
            }
            else
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }
    }
}
