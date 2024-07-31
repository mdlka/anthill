using System;
using System.Collections.Generic;
using YellowSquad.Anthill.Core.GameTime;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public class CollectHexTaskGroupFactory : ITaskGroupFactory
    {
        private readonly IHexMap _map;
        private readonly IHexMapView _mapView;
        private readonly IStopwatch _stopwatch;
        private readonly float _delayBetweenTasks;

        public CollectHexTaskGroupFactory(IHexMap map, IHexMapView mapView, IStopwatch stopwatch, float delayBetweenTasks)
        {
            _map = map;
            _mapView = mapView;
            _stopwatch = stopwatch;
            _delayBetweenTasks = delayBetweenTasks;
        }

        public bool CanCreate(AxialCoordinate targetPosition)
        {
            return _map.HasPosition(targetPosition) && 
                   _map.IsClosed(targetPosition) == false &&
                   _map.HexFrom(targetPosition).HasParts;
        }

        public ITaskGroup Create(AxialCoordinate targetPosition, Action onComplete = null)
        {
            if (CanCreate(targetPosition) == false)
                throw new InvalidOperationException();
            
            var tasks = new HashSet<ITask>();
            var targetHex = _map.HexFrom(targetPosition);
            var targetHexMatrix = _mapView.HexMatrixBy(_map.Scale, targetPosition);
                        
            foreach (var part in targetHex.Parts)
            {
                tasks.Add(new TaskWithCallback(
                    new TakePartTask(targetHexMatrix.MultiplyPoint(part.LocalPosition).ToFracAxialCoordinate(_map.Scale), targetHex, part), 
                    onComplete: () => 
                    { 
                        _map.Visualize(_mapView, new MapCellChange
                        {
                            Position = targetPosition,
                            AddedParts = Array.Empty<IReadOnlyPart>(),
                            RemovedParts = new[] {part},
                            MapCell = _map.MapCell(targetPosition),
                            ChangeType = ChangeType.Hex
                        });

                        if (_map.HexFrom(targetPosition).HasParts == false)
                        {
                            var neighborsCellChanges = new List<MapCellChange>(6);
                            var closedNeighborPositions = _map.NeighborHexPositions(targetPosition, pos => _map.IsClosed(pos));
                            
                            _map.UpdateClosedPositionNeighbor(targetPosition);

                            foreach (var neighborPosition in closedNeighborPositions)
                            {
                                if (_map.IsClosed(neighborPosition))
                                    continue;
                                
                                neighborsCellChanges.Add(new MapCellChange
                                {
                                    Position = neighborPosition,
                                    AddedParts = _map.HexFrom(neighborPosition).Parts,
                                    RemovedParts = Array.Empty<IReadOnlyPart>(),
                                    MapCell = _map.MapCell(neighborPosition),
                                    ChangeType = ChangeType.Hex
                                });
                            }
                            
                            _map.Visualize(_mapView, neighborsCellChanges.ToArray());
                        }

                        onComplete?.Invoke();
                    }));
            }

            return new TaskGroup(targetPosition, tasks, _stopwatch, _delayBetweenTasks);
        }
    }
}
