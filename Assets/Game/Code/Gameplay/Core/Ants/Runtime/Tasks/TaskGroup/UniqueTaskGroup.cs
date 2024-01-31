using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class UniqueTaskGroup : ITaskGroup
    {
        private readonly ITaskGroup _taskGroup;

        public UniqueTaskGroup(ITaskGroup taskGroup)
        {
            _taskGroup = taskGroup;
        }

        public AxialCoordinate TargetCellPosition => _taskGroup.TargetCellPosition;
        public bool HasFreeTask => _taskGroup.HasFreeTask;
        
        public ITask ClosestTask(FracAxialCoordinate position)
        {
            return _taskGroup.ClosestTask(position);
        }
        
        public bool Equals(ITaskGroup other)
        {
            return this == other;
        }
    }
}