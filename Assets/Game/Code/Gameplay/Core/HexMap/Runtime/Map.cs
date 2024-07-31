using System;
using System.Collections.Generic;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class Map : IHexMap
    {
        private readonly float _scale;
        private readonly Dictionary<AxialCoordinate, MapCell> _cells;
        private readonly HashSet<AxialCoordinate> _closedPositions = new();

        public Map(IReadOnlyDictionary<AxialCoordinate, MapCell> cells) : this(1f, cells) { }
        
        public Map(float scale, IReadOnlyDictionary<AxialCoordinate, MapCell> cells)
        {
            if (scale <= 0)
                throw new ArgumentOutOfRangeException(nameof(scale));
            
            _scale = scale;
            _cells = new Dictionary<AxialCoordinate, MapCell>(cells);
        }

        public float Scale => _scale;
        public int TotalCells => _cells.Count;
        public IEnumerable<AxialCoordinate> Positions => _cells.Keys;
        
        public void UpdateAllClosedPositions()
        {
            _closedPositions.Clear();
            
            foreach (var pair in _cells)
                if (CalculateClosed(pair.Key))
                    _closedPositions.Add(pair.Key);
        }

        public void UpdateClosedPositionNeighbor(AxialCoordinate position)
        {
            if (HasPosition(position) == false)
                throw new ArgumentOutOfRangeException();

            if (_closedPositions.Contains(position) && CalculateClosed(position) == false)
                _closedPositions.Remove(position);
            
            var neighborPositions = NeighborHexPositions(position);
            
            foreach (var neighborPosition in neighborPositions)
                if (_closedPositions.Contains(neighborPosition) && CalculateClosed(position) == false)
                    _closedPositions.Remove(neighborPosition);
        }

        public bool HasPosition(AxialCoordinate position)
        {
            return _cells.ContainsKey(position);
        }

        public bool HasObstacleIn(AxialCoordinate position)
        {
            if (HasPosition(position) == false)
                throw new ArgumentOutOfRangeException();

            return _cells[position].Hex.HasParts || 
                   _cells[position].PointOfInterestType != PointOfInterestType.Empty;
        }

        public bool IsClosed(AxialCoordinate position)
        {
            if (HasPosition(position) == false)
                throw new ArgumentOutOfRangeException();

            return _closedPositions.Contains(position);
        }

        public IHex HexFrom(AxialCoordinate position)
        {
            if (HasPosition(position) == false)
                throw new ArgumentOutOfRangeException();
            
            return _cells[position].Hex;
        }

        public MapCell MapCell(AxialCoordinate position)
        {
            return _cells[position];
        }

        public bool HasDividedPointOfInterestIn(AxialCoordinate position)
        {
            if (HasPosition(position) == false)
                throw new InvalidOperationException();

            return _cells[position].HasDividedPointOfInterest;
        }

        public IDividedPointOfInterest DividedPointOfInterestFrom(AxialCoordinate position)
        {
            if (HasDividedPointOfInterestIn(position) == false)
                throw new InvalidOperationException();

            return _cells[position].DividedPointOfInterest;
        }

        public PointOfInterestType PointOfInterestTypeIn(AxialCoordinate position)
        {
            if (HasPosition(position) == false)
                throw new InvalidOperationException();

            return _cells[position].PointOfInterestType;
        }

        public IReadOnlyList<AxialCoordinate> NeighborHexPositions(AxialCoordinate position, Func<AxialCoordinate, bool> where = null)
        {
            var neighborPositions = position.NeighborsPositions();
            var neighborMapPositions = new List<AxialCoordinate>(6);

            foreach (var neighborPosition in neighborPositions)
                if (HasPosition(neighborPosition) && (where?.Invoke(neighborPosition) ?? true))
                    neighborMapPositions.Add(neighborPosition);

            return neighborMapPositions;
        }

        public IReadOnlyList<AxialCoordinate> PointsOfInterestPositions(PointOfInterestType targetPoint)
        {
            var points = new List<AxialCoordinate>();

            foreach (var cell in _cells)
                if (cell.Value.PointOfInterestType == targetPoint) 
                    points.Add(cell.Key);

            return points;
        }
        
        public void Visualize(IHexMapView view, params MapCellChange[] changes)
        {
            var closedPositions = new HashSet<AxialCoordinate>();

            foreach (var cell in _cells)
                if (IsClosed(cell.Key)) // TODO: Need optimization, because called every frame
                    closedPositions.Add(cell.Key);

            if (view.Initialized == false)
                view.InitializeRender(_scale, _cells, closedPositions);
            else
                view.Render(_scale, closedPositions, changes);
        }

        public override string ToString()
        {
            return $"Count: {_cells.Count}\n{string.Join(' ', _cells)}";
        }

        private bool CalculateClosed(AxialCoordinate position)
        {
            return HasObstacleIn(position) &&
                   NeighborHexPositions(position, where: pos => HasObstacleIn(pos) == false).Count == 0;
        }
    }
}
