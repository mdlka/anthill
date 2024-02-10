using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public class AlwaysCompletedTask : ITask
    {
        public int Price => 0;
        public FracAxialCoordinate TargetPosition => default;
        public TaskState State => TaskState.Complete;
        public bool CanComplete => false;

        public void Execute() { }
        public void Complete() { }

        public bool Equals(ITask other)
        {
            return false;
        }
    }
}