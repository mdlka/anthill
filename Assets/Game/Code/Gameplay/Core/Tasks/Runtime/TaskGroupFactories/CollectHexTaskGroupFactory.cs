using System;
using System.Collections.Generic;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public class CollectHexTaskGroupFactory : ITaskGroupFactory
    {
        private readonly IHexMap _map;
        private readonly IHexMapView _mapView;

        public CollectHexTaskGroupFactory(IHexMap map, IHexMapView mapView)
        {
            _map = map;
            _mapView = mapView;
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
                        _map.Visualize(_mapView);
                        onComplete?.Invoke();
                    }));
            }

            return new TaskGroup(targetPosition, tasks);
        }
    }
}
