using System;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class AntHome : IHome
    {
        private readonly IReadOnlyTaskStorage _taskStorage;

        public AntHome(AxialCoordinate position, IReadOnlyTaskStorage taskStorage)
        {
            Position = position;
            _taskStorage = taskStorage;
        }

        public AxialCoordinate Position { get; }
        public bool HasTask => _taskStorage.HasTask;
        
        public ITask FindTask()
        {
            if (HasTask == false)
                throw new InvalidOperationException();
            
            return _taskStorage.FindTask();
        }
    }
}