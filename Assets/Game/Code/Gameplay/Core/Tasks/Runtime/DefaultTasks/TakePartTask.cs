using System;
using UnityEngine;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public class TakePartTask : BaseTask
    {
        private readonly IDividedObject _targetDividedObject;
        private readonly IReadOnlyPart _targetPart;
        
        private float _executeTime;
        private float _elapsedTime;

        public TakePartTask(FracAxialCoordinate targetPosition, IDividedObject targetDividedObject, IReadOnlyPart targetPart, int price = 0) 
            : base(price, targetPosition)
        {
            _targetDividedObject = targetDividedObject;
            _targetPart = targetPart;
        }

        protected override bool CanComplete => _elapsedTime >= (int)_targetDividedObject.Hardness + 1;

        public override bool Equals(ITask other)
        {
            return TargetPosition == other.TargetPosition;
        }

        protected override void OnUpdateProgress(float speed)
        {
            _elapsedTime += Time.unscaledDeltaTime * speed;
        }

        protected override void OnCompleted()
        {
            _targetDividedObject.DestroyClosestPartFor(_targetPart.LocalPosition);
        }
    }
}