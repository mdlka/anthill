using System;
using System.Threading.Tasks;

namespace YellowSquad.BehaviorTree
{
    public class Callback : INode
    {
        private readonly Action _action;

        public Callback(Action action)
        {
            _action = action;
        }

        public NodeStatus Status { get; private set; }

        public Task AsyncExecute()
        {
            _action?.Invoke();
            Status = NodeStatus.Success;
            
            return Task.CompletedTask;
        }

        public void Reset()
        {
            Status = NodeStatus.Idle;
        }
    }
}