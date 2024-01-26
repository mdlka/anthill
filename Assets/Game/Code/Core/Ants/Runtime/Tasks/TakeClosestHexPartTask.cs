using System;
using UnityEngine;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class TakeClosestHexPartTask : ITask
    {
        private readonly float _mapScale;
        private readonly IHex _targetHex;
        
        private FracAxialCoordinate _executePosition;
        private float _executeTime;

        public TakeClosestHexPartTask(float mapScale, AxialCoordinate targetCellPosition, IHex targetHex)
        {
            TargetCellPosition = targetCellPosition;
            _mapScale = mapScale;
            _targetHex = targetHex;
        }
        
        public AxialCoordinate TargetCellPosition { get; }
        public TaskState State { get; private set; }
        public bool CanComplete => State == TaskState.Executing && Time.realtimeSinceStartup - _executeTime >= (int)_targetHex.Hardness + 1;

        public void Execute(FracAxialCoordinate position)
        {
            if (State != TaskState.Idle)
                throw new InvalidOperationException("Already executed");

            _executePosition = position;
            _executeTime = Time.realtimeSinceStartup;
            
            State = TaskState.Executing;
        }

        public void Complete()
        {
            if (CanComplete == false)
                throw new InvalidOperationException();
            
            _targetHex.DestroyClosestPartFor((_executePosition - TargetCellPosition).ToVector3(_mapScale));
            State = TaskState.Complete;
        }

        public bool Equals(ITask other)
        {
            return false;
        }
    }
}