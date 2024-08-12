using System.Threading.Tasks;

namespace YellowSquad.BehaviorTree
{
    public class RepeatUntil : INode
    {
        private readonly INode _node;
        private readonly ICondition _untilCondition;

        public RepeatUntil(ICondition untilCondition, INode node)
        {
            _node = node;
            _untilCondition = untilCondition;
        }

        public NodeStatus Status { get; private set; }

        public async Task AsyncExecute()
        {
            while (TryCompleteNode() == false)
            {
                _node.Reset();
                
                await _node.AsyncExecute();
            }
        }

        public void Reset()
        {
            Status = NodeStatus.Idle;
        }

        private bool TryCompleteNode()
        {
            Status = _untilCondition.Completed ? NodeStatus.Success : Status;
            return Status == NodeStatus.Success;
        }
    }
}