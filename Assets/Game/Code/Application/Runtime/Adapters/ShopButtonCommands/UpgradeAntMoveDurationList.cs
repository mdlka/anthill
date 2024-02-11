using System;
using UnityEngine;

namespace YellowSquad.Anthill.Application
{
    internal class UpgradeAntMoveDurationList
    {
        private readonly int _upgradeCount;
        private readonly float _minMoveDuration;
        private readonly float _maxMoveDuration;
        private int _upgradeNumber = 1;

        public UpgradeAntMoveDurationList(int upgradeCount, float minMoveDuration, float maxMoveDuration)
        {
            if (upgradeCount <= 1)
                throw new ArgumentOutOfRangeException(nameof(upgradeCount));
            
            if (minMoveDuration < 0)
                throw new ArgumentOutOfRangeException(nameof(minMoveDuration));

            if (maxMoveDuration < 0)
                throw new ArgumentOutOfRangeException(nameof(maxMoveDuration));
            
            _upgradeCount = upgradeCount;
            _minMoveDuration = minMoveDuration;
            _maxMoveDuration = maxMoveDuration;

            CurrentValue = _maxMoveDuration;
        }

        public bool HasNext => _upgradeNumber < _upgradeCount;
        public float CurrentValue { get; private set; }

        public void Next()
        {
            if (HasNext == false)
                throw new InvalidOperationException();
            
            _upgradeNumber += 1;
            CurrentValue = Mathf.Lerp(_maxMoveDuration, _minMoveDuration, (float)(_upgradeNumber - 1) / (_upgradeCount - 1));
        }
    }
}