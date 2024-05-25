namespace PhantomDanmaku.Runtime
{
    public class GuidedBullet : BulletBase
    {
        protected override void Update()
        {
            if (owner != null && owner.transform != null)
            {
                dir = owner.transform.up;
            }

            base.Update();
        }
    }
}