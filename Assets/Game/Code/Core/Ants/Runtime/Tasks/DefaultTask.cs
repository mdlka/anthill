using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class DefaultTask : ITask
    {
        public DefaultTask(AxialCoordinate targetCellPosition)
        {
            TargetPosition = targetCellPosition;
        }
        
        public FracAxialCoordinate TargetPosition { get; }
        public TaskState State { get; private set; }
        public bool CanComplete => true;

        public void Execute()
        {
            State = TaskState.Executing;
        }

        public void Complete()
        {
            State = TaskState.Complete;
        }

        public bool Equals(ITask other)
        {
            return TargetPosition == other.TargetPosition;
        }
    }
}