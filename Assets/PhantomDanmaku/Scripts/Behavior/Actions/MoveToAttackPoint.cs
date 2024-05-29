using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace PhantomDanmaku.Runtime.Behavior
{
    public class MoveToAttackPoint : Action
    {
        private MonsterBase m_Monster;

        private Stack<Vector2> m_Path;

        private Vector2 m_CurPoint;

        /// <summary>
        /// 移动持续时间，如果时间到了还没到达目的地，则直接退出该行为
        /// </summary>
        private float m_DurationTime;

        private static Vector2 s_Offset = new Vector2(0.5f,0.5f);

        public override async void OnStart()
        {
            base.OnStart();
            if (Player.Instance == null)
                return;
            m_DurationTime = 2f;
            m_Monster = GetComponent<MonsterBase>();
            
            var pathFinder = new PathFinder();
            pathFinder.Init(MapGenerator.Instance.TilemapWall, m_Monster.V2IPos, Player.Instance.V2IPos);
            m_Path = await pathFinder.GetPathList();
            if (m_Path != null)
                m_CurPoint = m_Path.Pop() + s_Offset;
        }

        public override TaskStatus OnUpdate()
        {

            if (Player.Instance == null)
                return TaskStatus.Failure;
            #region 绘制调试路线

            m_Monster.LineRenderer.enabled = MonsterBase.ShowLine;
            
            if (m_Path != null && MonsterBase.ShowLine)
            {
                var pathList = m_Path.ToList();
                var points = new Vector3[m_Path.Count];
                for (var i = 0; i < pathList.Count; i++)
                {
                    points[i] = new Vector3(pathList[i].x, pathList[i].y, -2) + new Vector3(s_Offset.x, s_Offset.y, 0);
                }

                m_Monster.LineRenderer.positionCount = points.Length;
                m_Monster.LineRenderer.SetPositions(points);
            }

            #endregion
            
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
                m_CurPoint = m_Path.Pop() + s_Offset;
            }
            else
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }
    }
}
