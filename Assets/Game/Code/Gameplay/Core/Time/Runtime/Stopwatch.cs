using System;
using System.Collections.Generic;
using YellowSquad.GameLoop;

namespace YellowSquad.Anthill.Core.GameTime
{
    public class Stopwatch : IStopwatch, IGameLoop
    {
        private readonly List<float> _elapsedTimes = new();
        
        public int Create()
        {
            _elapsedTimes.Add(0f);
            return _elapsedTimes.Count - 1;
        }

        public void Restart(int index)
        {
            ThrowIfNoIndex(index);
            _elapsedTimes[index] = 0;
        }

        public float ElapsedTime(int index)
        {
            ThrowIfNoIndex(index);
            return _elapsedTimes[index];
        }

        public void Update(float deltaTime)
        {
            for (int i = 0; i < _elapsedTimes.Count; i++)
                _elapsedTimes[i] += deltaTime;
        }

        private void ThrowIfNoIndex(int index)
        {
            if (index < 0 || index >= _elapsedTimes.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
        }
    }
}