using System;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class TaskWithAction : ITask
    {
        private readonly ITask _task;
        private readonly Action _action;

        public TaskWithAction(ITask task, Action action)
        {
            _task = task;
            _action = action;
        }

        public AxialCoordinate TargetCellPosition => _task.TargetCellPosition;
        public bool Completed => _task.Completed;
        
        public void Complete()
        {
            _task.Complete();
            _action?.Invoke();
        }
        
        public bool Equals(ITask other)
        {
            return _task.Equals(other);
        }
    }
}