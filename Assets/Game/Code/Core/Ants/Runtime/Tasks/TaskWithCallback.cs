using System;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class TaskWithCallback : ITask
    {
        private readonly ITask _task;
        private readonly Action _onComplete;

        public TaskWithCallback(ITask task, Action onComplete)
        {
            _task = task;
            _onComplete = onComplete;
        }

        public AxialCoordinate TargetCellPosition => _task.TargetCellPosition;
        public bool Completed => _task.Completed;
        
        public void Complete()
        {
            _task.Complete();
            _onComplete?.Invoke();
        }
        
        public bool Equals(ITask other)
        {
            return _task.Equals(other);
        }
    }
}