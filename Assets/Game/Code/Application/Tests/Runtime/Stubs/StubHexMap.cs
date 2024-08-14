using System;
using System.Collections.Generic;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Application.Tests
{
    internal class StubHexMap : IHexMap
    {
        // Has an obstacle if value is true
        private readonly Dictionary<AxialCoordinate, bool> _mapWithObstacles;

        public StubHexMap(Dictionary<AxialCoordinate, bool> mapWithObstacles)
        {
            _mapWithObstacles = mapWithObstacles;
        }

        public float Scale => 1f;
        public int TotalCells => _mapWithObstacles.Count;
        public IEnumerable<AxialCoordinate> Positions { get; }

        public void UpdateAllClosedPositions()
        {
            throw new NotImplementedException();
        }

        public void UpdateClosedPositionNeighbor(AxialCoordinate position)
        {
            throw new NotImplementedException();
        }

        public bool HasPosition(AxialCoordinate position)
        {
            return _mapWithObstacles.ContainsKey(position);
        }

        public bool HasObstacleIn(AxialCoordinate position)
        {
            if (HasPosition(position) == false)
                throw new ArgumentOutOfRangeException();

            return _mapWithObstacles[position];
        }

        public bool IsClosed(AxialCoordinate position)
        {
            throw new NotImplementedException();
        }

        public IHex HexFrom(AxialCoordinate position)
        {
            throw new NotImplementedException();
        }

        public MapCell MapCell(AxialCoordinate position)
        {
            throw new NotImplementedException();
        }

        public bool HasDividedPointOfInterestIn(AxialCoordinate position)
        {
            throw new NotImplementedException();
        }

        public IDividedPointOfInterest DividedPointOfInterestFrom(AxialCoordinate position)
        {
            throw new NotImplementedException();
        }

        public PointOfInterestType PointOfInterestTypeIn(AxialCoordinate position)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<AxialCoordinate> NeighborHexPositions(AxialCoordinate position, Func<AxialCoordinate, bool> where = null)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<AxialCoordinate> PointsOfInterestPositions(PointOfInterestType targetPoint)
        {
            throw new NotImplementedException();
        }

        public void Visualize(IHexMapView view, params MapCellChange[] changes)
        {
            throw new NotImplementedException();
        }
    }
}