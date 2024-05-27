using BehaviorDesigner.Runtime.Tasks;

namespace PhantomDanmaku.Runtime.Behavior
{
    public class PlayerEnterRoom : Conditional
    {
        public override TaskStatus OnUpdate()
        {
            if (Player.Instance.CurRoom == transform.GetComponent<MonsterBase>().CurRoom)
            {
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }
    }
}