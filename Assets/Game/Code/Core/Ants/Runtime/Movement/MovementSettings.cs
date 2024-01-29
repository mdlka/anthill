using System;
using UnityEngine;
using YellowSquad.HexMath;
using Random = UnityEngine.Random;

namespace YellowSquad.Anthill.Core.Ants
{
    [CreateAssetMenu(menuName = "Anthill/Movement/Create MovementSettings", fileName = "MovementSettings", order = 56)]
    public class MovementSettings : ScriptableObject
    {
        [SerializeField, Min(1)] private int _stepsToGoal = 10;
        [SerializeField, Min(0f)] private float _moveToGoalDuration = 0.3f;
        [SerializeField, Range(0f, 1f)] private float _randomOffsetRadius = 0.45f;

        [NonSerialized] private bool _initialized;
        private float _mapScale;

        public int StepsToGoal => _stepsToGoal;
        public float NormalizedMoveDuration => _moveToGoalDuration / _stepsToGoal;
        public float MaxRandomOffsetRadius => _randomOffsetRadius;
        
        public void Initialize(float mapScale)
        {
            if (_initialized)
                throw new InvalidOperationException();
            
            _mapScale = mapScale;
            _initialized = true;
        }

        internal FracAxialCoordinate RandomOffset()
        {
            if (_initialized == false)
                throw new InvalidOperationException("Not initialized");
            
            return (Random.insideUnitCircle * _randomOffsetRadius).ToFracAxialCoordinate(_mapScale);
        }
    }
}