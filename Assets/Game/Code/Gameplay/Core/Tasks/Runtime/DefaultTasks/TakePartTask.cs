using System;
using UnityEngine;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public class TakePartTask : ITask
    {
        private readonly IDividedObject _targetDividedObject;
        private readonly IPart _targetPart;
        private readonly Action _onProgress;
        private readonly float _targetDuration;

        private float _elapsedTime;
        private bool _onCompletedInvoked;

        public TakePartTask(FracAxialCoordinate targetPosition, IDividedObject targetDividedObject, IPart targetPart, int price = 0, Action onProgress = null)
        {
            if (price < 0)
                throw new ArgumentOutOfRangeException(nameof(price));
            
            Price = price;
            TargetPosition = targetPosition;
            _targetDividedObject = targetDividedObject;
            _targetPart = targetPart;
            _onProgress = onProgress;

            _targetDuration = ((int)_targetDividedObject.Hardness + 1) * 2f;
        }

        public int Price { get; }
        public FracAxialCoordinate TargetPosition { get; }
        public bool Completed => CanComplete && _onCompletedInvoked;
        public bool Cancelled { get; private set; }
        private bool CanComplete => _elapsedTime >= _targetDuration;
        
        public void UpdateProgress(float deltaTime)
        {
            if (Cancelled)
                throw new InvalidOperationException("Task is cancelled");
            
            if (Completed)
                throw new InvalidOperationException("Task is complete");
            
            if (deltaTime < 0)
                throw new ArgumentOutOfRangeException(nameof(deltaTime));

            _elapsedTime += deltaTime;
            _targetPart.Resize(Mathf.Lerp(1f, 0.5f, _elapsedTime / _targetDuration));
            _onProgress?.Invoke();

            if (CanComplete == false || _onCompletedInvoked) 
                return;
            
            _targetDividedObject.DestroyClosestPartFor(_targetPart.LocalPosition);
            _onCompletedInvoked = true;
        }

        public void Cancel()
        {
            if (Cancelled)
                throw new InvalidOperationException();
            
            _targetPart.Resize(1f);
            _onProgress?.Invoke();
            Cancelled = true;
        }

        public bool Equals(ITask other)
        {
            return TargetPosition == other.TargetPosition;
        }
    }
}