using System;
using YellowSquad.Anthill.Core.GameTime;
using YellowSquad.Anthill.Core.Tasks;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class AntHome : IHome
    {
        private readonly float _delayBetweenFindTasks;
        private readonly IReadOnlyTaskStorage _taskStorage;
        private readonly IStopwatch _stopwatch;
        private readonly int _stopwatchIndex; 

        public AntHome(AxialCoordinate position, IReadOnlyTaskStorage taskStorage, IStopwatch stopwatch, float delayBetweenFindTasks)
        {
            Position = position;
            _taskStorage = taskStorage;
            _stopwatch = stopwatch;
            _delayBetweenFindTasks = delayBetweenFindTasks;

            _stopwatchIndex = _stopwatch.Create();
        }

        public AxialCoordinate Position { get; }
        public bool HasFreeTaskGroup => _taskStorage.HasFreeTaskGroup && _stopwatch.ElapsedTime(_stopwatchIndex) >= _delayBetweenFindTasks;

        public ITaskGroup FindTaskGroup()
        {
            if (HasFreeTaskGroup == false)
                throw new InvalidOperationException();

            _stopwatch.Restart(_stopwatchIndex);
            return _taskStorage.FindTaskGroup();
        }
    }
}