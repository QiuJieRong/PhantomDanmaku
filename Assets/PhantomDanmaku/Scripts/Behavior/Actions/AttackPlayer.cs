using BehaviorDesigner.Runtime.Tasks;

namespace PhantomDanmaku.Runtime.Behavior
{
    public class AttackPlayer : Action
    {
        private bool m_AttackComplete;

        private MonsterBase m_Monster;

        public override async void OnStart()
        {
            base.OnStart();
            m_AttackComplete = false;
            m_Monster = GetComponent<MonsterBase>();
            await m_Monster.AttackForBehaviorTree();
            m_AttackComplete = true;
        }

        public override TaskStatus OnUpdate()
        {
            return m_AttackComplete ? TaskStatus.Success : TaskStatus.Running;
        }
    }
}