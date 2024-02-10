using System;
using UnityEngine;
using YellowSquad.Anthill.Core.Tasks;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class AntHome : IHome
    {
        private readonly float _delayBetweenFindTasks;
        private readonly IReadOnlyTaskStorage _taskStorage;
        
        private float _lastFindTaskTime;

        public AntHome(AxialCoordinate position, IReadOnlyTaskStorage taskStorage, float delayBetweenFindTasks)
        {
            Position = position;
            _taskStorage = taskStorage;
            _delayBetweenFindTasks = delayBetweenFindTasks;
        }

        public AxialCoordinate Position { get; }
        public bool HasFreeTaskGroup => _taskStorage.HasFreeTaskGroup && Time.realtimeSinceStartup - _lastFindTaskTime >= _delayBetweenFindTasks;

        public ITaskGroup FindTaskGroup()
        {
            if (HasFreeTaskGroup == false)
                throw new InvalidOperationException();

            _lastFindTaskTime = Time.realtimeSinceStartup;
            return _taskStorage.FindTaskGroup();
        }
    }
}