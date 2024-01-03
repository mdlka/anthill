using System;
using UnityEngine;
using YellowSquad.HexMath;
using Random = UnityEngine.Random;

namespace YellowSquad.Anthill.Core.Ants
{
    [CreateAssetMenu(menuName = "Anthill/Movement/Create MovementSettings", fileName = "MovementSettings", order = 56)]
    public class MovementSettings : ScriptableObject
    {
        [field: SerializeField, Min(1)] public int StepsToGoal { get; private set; }
        [field: SerializeField, Min(0f)] public float MoveToGoalDuration { get; private set; }
        [field: SerializeField, Range(0f, 1f)] public float RandomOffsetRadius { get; private set; }

        [NonSerialized] private bool _initialized;
        private float _mapScale;
        
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
            
            return (Random.insideUnitCircle * RandomOffsetRadius).ToFracAxialCoordinate(_mapScale);
        }
    }
}