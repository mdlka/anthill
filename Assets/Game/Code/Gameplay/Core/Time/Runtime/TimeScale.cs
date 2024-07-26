using System;
using UnityEngine;

namespace YellowSquad.Anthill.Core.GameTime
{
    [CreateAssetMenu(menuName = "Anthill/Time/Create TimeScale", fileName = "TimeScale", order = 56)]
    public class TimeScale : ScriptableObject
    {
        [field: NonSerialized] public float Value { get; private set; } = 1;

        public void ChangeValue(float value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            Value = value;
        }
    }
}
