using System.Collections.Generic;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class LeafTasksLoop : IGameLoop
    {
        private readonly IHexMap _map;
        private readonly IHexMapView _mapView;
        private readonly ITaskStorage _targetTaskStorage;
        private readonly IReadOnlyList<AxialCoordinate> _leafPositions;
        private readonly Dictionary<AxialCoordinate, ITaskGroup> _tasks = new();

        public LeafTasksLoop(IHexMap map, IHexMapView mapView, ITaskStorage targetTaskStorage)
        {
            _map = map;
            _mapView = mapView;
            _targetTaskStorage = targetTaskStorage;
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
                
                var tasks = new HashSet<ITask>();
                var pointOfInterestMatrix = _mapView.PointOfInterestMatrixBy(_map.Scale, 
                    position, _map.PointOfInterestTypeIn(position));
                                    
                foreach (var part in leaf.Parts)
                {
                    tasks.Add(new TaskWithCallback(
                        new TakePartTask(pointOfInterestMatrix.MultiplyPoint(part.LocalPosition).ToFracAxialCoordinate(_map.Scale), leaf, part), 
                        onComplete: () => _map.Visualize(_mapView)));
                }

                var taskGroup = new TaskGroup(position, tasks);
                _targetTaskStorage.AddTaskGroup(taskGroup);
                _tasks.Add(position, taskGroup);
            }
        }
    }
}
