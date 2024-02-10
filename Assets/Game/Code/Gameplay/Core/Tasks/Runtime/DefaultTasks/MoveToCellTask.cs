using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public class MoveToCellTask : ITask
    {
        public MoveToCellTask(AxialCoordinate targetCellPosition)
        {
            TargetPosition = targetCellPosition;
        }

        public int Price => 0;
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