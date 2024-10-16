﻿using System;
using System.Collections.Generic;
using YellowSquad.Anthill.Core.GameTime;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public class CollectPointOfInterestTaskGroupFactory : ITaskGroupFactory
    {
        private readonly IHexMap _map;
        private readonly IHexMapView _mapView;
        private readonly IStopwatch _stopwatch;
        private readonly float _delayBetweenTasks;
        private readonly int _taskPrice;

        public CollectPointOfInterestTaskGroupFactory(IHexMap map, IHexMapView mapView, IStopwatch stopwatch, float delayBetweenTasks, int taskPrice)
        {
            _map = map;
            _mapView = mapView;
            _stopwatch = stopwatch;
            _delayBetweenTasks = delayBetweenTasks;
            _taskPrice = taskPrice;
        }

        public bool CanCreate(AxialCoordinate targetPosition)
        {
            return _map.HasPosition(targetPosition) && 
                   _map.IsClosed(targetPosition) == false &&
                   _map.HexFrom(targetPosition).HasParts == false &&
                   _map.HasDividedPointOfInterestIn(targetPosition);
        }

        public ITaskGroup Create(AxialCoordinate targetPosition, Action onComplete = null)
        {
            if (CanCreate(targetPosition) == false)
                throw new InvalidOperationException();
            
            var tasks = new HashSet<ITask>();
            var targetPointOfInterest = _map.DividedPointOfInterestFrom(targetPosition);
            var targetMatrix = _mapView.PointOfInterestMatrixBy(_map.Scale, targetPosition,  _map.PointOfInterestTypeIn(targetPosition));
                        
            foreach (var part in targetPointOfInterest.Parts)
            {
                tasks.Add(new TaskWithCallback(
                    new TakePartTask(targetMatrix.MultiplyPoint(part.LocalPosition).ToFracAxialCoordinate(_map.Scale), targetPointOfInterest, part, _taskPrice, 
                        onProgress: () =>
                        {
                            _map.Visualize(_mapView, new MapCellChange
                            {
                                Position = targetPosition,
                                ChangedSizeParts = new[] {part},
                                MapCell = _map.MapCell(targetPosition),
                                ChangeType = ChangeType.PointOfInterest
                            });
                        }), 
                    onComplete: () => 
                    { 
                        _map.Visualize(_mapView, new MapCellChange
                        {
                            Position = targetPosition,
                            RemovedParts = new[] {part},
                            MapCell = _map.MapCell(targetPosition),
                            ChangeType = ChangeType.PointOfInterest
                        });
                        onComplete?.Invoke();
                    }));
            }

            return new TaskGroup(targetPosition, tasks, _stopwatch, _delayBetweenTasks);
        }
    }
}