using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class DefaultTask : ITask
    {
        public DefaultTask(AxialCoordinate targetCellPosition)
        {
            TargetCellPosition = targetCellPosition;
        }
        
        public AxialCoordinate TargetCellPosition { get; }
        public bool Completed { get; private set; }
        
        public void Complete()
        {
            Completed = true;
        }

        public bool Equals(ITask other)
        {
            return false;
        }
    }
}