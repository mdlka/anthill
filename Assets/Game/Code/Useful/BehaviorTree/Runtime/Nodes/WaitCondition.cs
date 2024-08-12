using System.Threading.Tasks;

namespace YellowSquad.BehaviorTree
{
    public class WaitCondition : INode
    {
        private readonly ICondition _condition;

        public WaitCondition(ICondition condition)
        {
            _condition = condition;
        }

        public NodeStatus Status { get; private set; }

        public async Task AsyncExecute()
        {
            Status = NodeStatus.Running;
            
            while (_condition.Completed == false)
                await Task.Yield();
            
            Status = NodeStatus.Success;
        }

        public void Reset()
        {
            Status = NodeStatus.Idle;
        }
    }
}
