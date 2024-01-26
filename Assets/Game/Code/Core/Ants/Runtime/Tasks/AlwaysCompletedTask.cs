using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    internal class AlwaysCompletedTask : ITask
    {
        public AxialCoordinate TargetCellPosition => default;
        public TaskState State => TaskState.Complete;
        public bool CanComplete => false;

        public void Execute(FracAxialCoordinate _) { }
        public void Complete() { }

        public bool Equals(ITask other)
        {
            return false;
        }
    }
}