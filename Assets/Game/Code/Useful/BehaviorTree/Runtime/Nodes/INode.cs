using System.Threading.Tasks;

namespace YellowSquad.BehaviorTree
{
    public interface INode
    {
        NodeStatus Status { get; }

        Task AsyncExecute();
        void Reset();
    }
}
