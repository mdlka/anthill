using System;
using UnityEngine;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class TakeHexPartTask : ITask
    {
        private readonly IHex _targetHex;
        private readonly IReadOnlyHexPart _targetPart;
        
        private float _executeTime;

        public TakeHexPartTask(FracAxialCoordinate targetPosition, IHex targetHex, IReadOnlyHexPart targetPart)
        {
            TargetPosition = targetPosition;
            _targetHex = targetHex;
            _targetPart = targetPart;
        }
        
        public FracAxialCoordinate TargetPosition { get; }
        public TaskState State { get; private set; }
        public bool CanComplete => State == TaskState.Executing && Time.realtimeSinceStartup - _executeTime >= (int)_targetHex.Hardness + 1;

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
            
            _targetHex.DestroyClosestPartFor(_targetPart.LocalPosition);
            State = TaskState.Complete;
        }

        public bool Equals(ITask other)
        {
            return TargetPosition == other.TargetPosition;
        }
    }
}