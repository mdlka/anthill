using System;
using UnityEngine;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public class TakePartTask : ITask
    {
        private readonly IDividedObject _targetDividedObject;
        private readonly IReadOnlyPart _targetPart;

        private float _elapsedTime;
        private bool _onCompletedInvoked;

        public TakePartTask(FracAxialCoordinate targetPosition, IDividedObject targetDividedObject, IReadOnlyPart targetPart, int price = 0)
        {
            if (price < 0)
                throw new ArgumentOutOfRangeException(nameof(price));
            
            Price = price;
            TargetPosition = targetPosition;
            _targetDividedObject = targetDividedObject;
            _targetPart = targetPart;
        }

        public int Price { get; }
        public FracAxialCoordinate TargetPosition { get; }
        public bool Completed => CanComplete && _onCompletedInvoked;
        private bool CanComplete => _elapsedTime >= (int)_targetDividedObject.Hardness + 1;
        
        public void UpdateProgress(float speed = 1f)
        {
            if (Completed)
                throw new InvalidOperationException("Task is complete");
            
            if (speed <= 0)
                throw new ArgumentOutOfRangeException(nameof(speed));

            _elapsedTime += Time.unscaledDeltaTime * speed;

            if (CanComplete == false || _onCompletedInvoked) 
                return;
            
            _targetDividedObject.DestroyClosestPartFor(_targetPart.LocalPosition);
            _onCompletedInvoked = true;
        }

        public bool Equals(ITask other)
        {
            return TargetPosition == other.TargetPosition;
        }
    }
}