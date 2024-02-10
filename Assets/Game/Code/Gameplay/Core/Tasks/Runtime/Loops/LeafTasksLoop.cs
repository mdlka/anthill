using System.Collections.Generic;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.GameLoop;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public class LeafTasksLoop : IGameLoop
    {
        private readonly IHexMap _map;
        private readonly ITaskStorage _targetTaskStorage;
        private readonly ITaskGroupFactory _collectLeafTaskGroupFactory;
        private readonly IReadOnlyList<AxialCoordinate> _leafPositions;
        private readonly Dictionary<AxialCoordinate, ITaskGroup> _tasks = new();

        public LeafTasksLoop(IHexMap map, ITaskStorage targetTaskStorage, ITaskGroupFactory collectLeafTaskGroupFactory)
        {
            _map = map;
            _targetTaskStorage = targetTaskStorage;
            _collectLeafTaskGroupFactory = collectLeafTaskGroupFactory;
            _leafPositions = map.PointsOfInterestPositions(PointOfInterestType.Leaf);
        }
        
        public void Update(float deltaTime)
        {
            foreach (var position in _leafPositions)
            {
                if (_map.HexFrom(position).HasParts)
                    continue;

                if (_tasks.ContainsKey(position) && _tasks[position].HasFreeTask)
                    continue;

                var leaf = _map.DividedPointOfInterestFrom(position);

                if (_tasks.ContainsKey(position) && _tasks[position].HasFreeTask == false)
                {
                    if (leaf.HasParts)
                        continue;
                    
                    _tasks.Remove(position);
                }
                
                if (leaf.HasParts == false)
                    continue;
                
                if (_collectLeafTaskGroupFactory.CanCreate(position) == false)
                    continue;

                var taskGroup = _collectLeafTaskGroupFactory.Create(position);
                _targetTaskStorage.AddTaskGroup(taskGroup);
                _tasks.Add(position, taskGroup);
            }
        }
    }
}
