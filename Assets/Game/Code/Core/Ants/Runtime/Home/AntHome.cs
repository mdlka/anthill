using System;
using UnityEngine;
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
        //public bool HasTask => _taskStorage.HasTask && Time.realtimeSinceStartup - _lastFindTaskTime >= _delayBetweenFindTasks;
        public bool HasFreeTaskGroup => _taskStorage.HasFreeTaskGroup;

        public ITaskGroup FindTaskGroup()
        {
            if (HasFreeTaskGroup == false)
                throw new InvalidOperationException();

            return _taskStorage.FindTaskGroup();
        }
    }
}