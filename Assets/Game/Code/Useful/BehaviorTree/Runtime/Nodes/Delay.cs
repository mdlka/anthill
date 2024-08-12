using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace YellowSquad.BehaviorTree
{
    public class Delay : INode
    {
        private readonly float _duration;

        public Delay(float duration)
        {
            _duration = duration;
        }

        public NodeStatus Status { get; private set; }

        public async Task AsyncExecute()
        {
            if (Status == NodeStatus.Running)
                throw new InvalidOperationException("Node running");

            Status = NodeStatus.Running;

            await UniTask.Delay((int)(_duration * 1000));
            
            Status = NodeStatus.Success;
        }

        public void Reset()
        {
            Status = NodeStatus.Idle;
        }
    }
}
