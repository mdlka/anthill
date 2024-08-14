using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using YellowSquad.GamePlatformSdk;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class Map : IHexMap
    {
        private readonly float _scale;
        private readonly ISave _save;
        private readonly MapSave _saveData;
        private readonly Dictionary<AxialCoordinate, MapCell> _cells;
        private readonly HashSet<AxialCoordinate> _closedPositions = new();

        public Map(IReadOnlyDictionary<AxialCoordinate, MapCell> cells, ISave save, MapSave saveData) : this(1f, cells, save, saveData) { }
        
        public Map(float scale, IReadOnlyDictionary<AxialCoordinate, MapCell> cells, ISave save, MapSave saveData)
        {
            if (scale <= 0)
                throw new ArgumentOutOfRangeException(nameof(scale));
            
            _scale = scale;
            _save = save;
            _saveData = saveData;
            _cells = new Dictionary<AxialCoordinate, MapCell>(cells);
        }

        public float Scale => _scale;
        public int TotalCells => _cells.Count;
        public IEnumerable<AxialCoordinate> Positions => _cells.Keys;

        public void UpdateAllClosedPositions()
        {
            _closedPositions.Clear();

            foreach (var pair in _cells)
            {
                if (CalculateClosed(pair.Key))
                    _closedPositions.Add(pair.Key);
                
                if (pair.Value.Hex.HasParts == false)
                    _saveData.OpenPositions.Add(pair.Key);
            }
        }

        public void UpdateClosedPositionNeighbor(AxialCoordinate position)
        {
            if (HasPosition(position) == false)
                throw new ArgumentOutOfRangeException();

            if (_cells[position].Hex.HasParts == false) 
            {
                // Band-aid. Because this method is called after a hex is broken
                _saveData.OpenPositions.Add(position);
                _save.SetString(SaveConstants.MapSaveKey, JsonConvert.SerializeObject(_saveData));
            }

            if (_closedPositions.Contains(position) && CalculateClosed(position) == false)
                _closedPositions.Remove(position);
            
            var openedNeighborPositions = NeighborHexPositions(position, 
                where: pos => _closedPositions.Contains(pos) && CalculateClosed(pos) == false);

            foreach (var neighborPosition in openedNeighborPositions)
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
            if (view.Initialized == false)
                view.InitializeRender(_scale, _cells, _closedPositions);
            else
                view.Render(_scale, _closedPositions, changes);
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
    
    [Serializable]
    internal class MapSave
    {
        [JsonProperty] public HashSet<AxialCoordinateSave> OpenPositions = new();
    }
}
