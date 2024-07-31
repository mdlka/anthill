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
        public bool Removed => _task.Removed;

        public void UpdateProgress(float deltaTime)
        {
            _task.UpdateProgress(deltaTime);
            
            if (Completed == false || _invoked)
                return;

            _onComplete?.Invoke();
            _invoked = true;
        }

        public void Remove()
        {
            _task.Remove();
        }

        public bool Equals(ITask other)
        {
            return _task.Equals(other);
        }
    }
}