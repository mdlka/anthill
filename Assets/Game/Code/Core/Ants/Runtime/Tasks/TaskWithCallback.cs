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
        public TaskState State => _task.State;
        public bool CanComplete => _task.CanComplete;
        
        public void Execute(FracAxialCoordinate position)
        {
            _task.Execute(position);
        }

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