﻿using System;
using UnityEngine;
using YellowSquad.HexMath;
using Random = UnityEngine.Random;

namespace YellowSquad.Anthill.Core.Ants
{
    [CreateAssetMenu(menuName = "Anthill/Movement/Create MovementSettings", fileName = "MovementSettings", order = 56)]
    public class MovementSettings : ScriptableObject
    {
        [SerializeField, Min(1)] private int _stepsBetweenCells = 15;
        [SerializeField, Min(0f)] private float _moveDurationBetweenCells = 0.4f;
        [SerializeField, Range(0f, 1f)] private float _randomOffsetRadius = 0.15f;

        [NonSerialized] private bool _initialized;
        private float _mapScale;

        public int StepsBetweenCells => _stepsBetweenCells;
        public float NormalizedMoveDuration => _moveDurationBetweenCells / _stepsBetweenCells;
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