using System;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public abstract class BaseTask : ITask
    {
        private bool _onCompletedInvoked;
        
        protected BaseTask(int price, FracAxialCoordinate targetPosition)
        {
            if (price < 0)
                throw new ArgumentOutOfRangeException(nameof(price));
            
            Price = price;
            TargetPosition = targetPosition;
        }
        
        public int Price { get; }
        public FracAxialCoordinate TargetPosition { get; }
        public bool Completed => CanComplete && _onCompletedInvoked;
        protected abstract bool CanComplete { get; }
        
        public void UpdateProgress(float speed = 1f)
        {
            if (Completed)
                throw new InvalidOperationException("Task is complete");
            
            if (speed <= 0)
                throw new ArgumentOutOfRangeException(nameof(speed));

            OnUpdateProgress(speed);

            if (CanComplete == false || _onCompletedInvoked) 
                return;
            
            OnCompleted();
            _onCompletedInvoked = true;
        }

        public abstract bool Equals(ITask other);
        protected abstract void OnUpdateProgress(float speed);
        protected abstract void OnCompleted();
    }
}