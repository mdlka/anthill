using System;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public class TaskWithCallback : ITask
    {
        private readonly ITask _task;
        private readonly Action _onComplete;

        private bool _invoked;

        public TaskWithCallback(ITask task, Action onComplete)
        {
            _task = task;
            _onComplete = onComplete;
        }

        public int Price => _task.Price;
        public FracAxialCoordinate TargetPosition => _task.TargetPosition;
        public bool Completed => _task.Completed;
        
        public void UpdateProgress(float speed)
        {
            _task.UpdateProgress(speed);
            
            if (Completed == false || _invoked)
                return;

            _onComplete?.Invoke();
            _invoked = true;
        }

        public bool Equals(ITask other)
        {
            return _task.Equals(other);
        }
    }
}