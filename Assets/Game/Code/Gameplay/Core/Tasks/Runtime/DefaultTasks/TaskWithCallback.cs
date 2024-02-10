using System;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
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

        public int Price => _task.Price;
        public FracAxialCoordinate TargetPosition => _task.TargetPosition;
        public TaskState State => _task.State;
        public bool CanComplete => _task.CanComplete;
        
        public void Execute()
        {
            _task.Execute();
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