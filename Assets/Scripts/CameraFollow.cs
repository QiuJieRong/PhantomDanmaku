using UnityEngine;

namespace PhantomDanmaku
{
    

    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float smoothTime = 1;
        private Vector3 speed;

        void Start()
        {

        }

        void Update()
        {
            if (target != null)
                transform.position = Vector3.SmoothDamp(transform.position, target.position + Vector3.back, ref speed,
                    smoothTime);
        }
    }

}