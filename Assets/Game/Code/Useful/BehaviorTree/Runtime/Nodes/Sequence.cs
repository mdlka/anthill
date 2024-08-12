using System;
using System.Threading.Tasks;

namespace YellowSquad.BehaviorTree
{
    public class Sequence : INode
    {
        private readonly INode[] _tutorialNodes;
        private readonly ICondition _skipCondition;

        public Sequence(ICondition skipCondition = null, params INode[] tutorialNodes) : this(tutorialNodes, skipCondition) { }
        public Sequence(params INode[] tutorialNodes) : this(tutorialNodes, null) { }
        
        public Sequence(INode[] tutorialNodes, ICondition skipCondition = null)
        {
            _tutorialNodes = tutorialNodes;
            _skipCondition = skipCondition;
        }

        public NodeStatus Status { get; private set; }

        public async Task AsyncExecute()
        {
            if (Status == NodeStatus.Running)
                throw new InvalidOperationException("Node running");

            Status = NodeStatus.Running;

            if (_skipCondition is { Completed: true })
            {
                Status = NodeStatus.Success;
                return;
            }

            foreach (var node in _tutorialNodes)
            {
                await node.AsyncExecute();
            }

            Status = NodeStatus.Success;
        }

        public void Reset()
        {
            foreach (var node in _tutorialNodes)
                node.Reset();

            Status = NodeStatus.Idle;
        }
    }
}
