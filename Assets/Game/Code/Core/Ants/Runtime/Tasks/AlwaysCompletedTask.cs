using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    internal class AlwaysCompletedTask : ITask
    {
        public AlwaysCompletedTask(AxialCoordinate targetPosition = default)
        {
            TargetCellPosition = targetPosition;
        }

        public AxialCoordinate TargetCellPosition { get; }
        public bool Completed => true;
        
        public void Complete() { }
        
        public bool Equals(ITask other)
        {
            return false;
        }
    }
}