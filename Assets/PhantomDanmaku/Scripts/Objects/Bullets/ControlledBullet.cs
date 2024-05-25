using UnityEngine;

namespace PhantomDanmaku.Runtime
{
    public class ControlledBullet : BulletBase
    {
        private Vector2 m_LastDir;

        private bool m_LeftMouseButtonDown = false;
        protected override void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_LeftMouseButtonDown = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                m_LeftMouseButtonDown = false;
            }

            //如果左键按下才会追踪
            if (m_LeftMouseButtonDown)
            {
                //获得鼠标的世界坐标
                var mousePos = Input.mousePosition + Vector3.forward * 5;
                var mousePosWs = (Vector2)Camera.main.ScreenToWorldPoint(mousePos);

                var pos = (Vector2)transform.position;
            
                //子弹向该位置靠近,距离越远速度越快
                var dis = Vector2.Distance(mousePosWs, pos);
                var guideSpeed = Mathf.Sqrt(dis) * 20;
                //获得方向
                var guideDir = (mousePosWs - pos).normalized;
                rig2D.velocity = guideSpeed * guideDir;
                m_LastDir = guideDir;
            }
            else
            {
                rig2D.velocity = m_LastDir * speed;
            }
        }
    }
}