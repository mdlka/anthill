using System;
using UnityEngine;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class TakePartTask : ITask
    {
        private readonly IDividedObject _targetDividedObject;
        private readonly IReadOnlyPart _targetPart;
        
        private float _executeTime;

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
        public TaskState State { get; private set; }
        public bool CanComplete => State == TaskState.Executing && Time.realtimeSinceStartup - _executeTime >= (int)_targetDividedObject.Hardness + 1;

        public void Execute()
        {
            if (State != TaskState.Idle)
                throw new InvalidOperationException("Already executed");

            _executeTime = Time.realtimeSinceStartup;
            
            State = TaskState.Executing;
        }

        public void Complete()
        {
            if (CanComplete == false)
                throw new InvalidOperationException();
            
            _targetDividedObject.DestroyClosestPartFor(_targetPart.LocalPosition);
            State = TaskState.Complete;
        }

        public bool Equals(ITask other)
        {
            return TargetPosition == other.TargetPosition;
        }
    }
}