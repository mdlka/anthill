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

        public IHex HexFrom(AxialCoordinate position)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<AxialCoordinate> PointsOfInterestPositions(PointOfInterest targetPoint)
        {
            throw new NotImplementedException();
        }

        public void Visualize(IHexMapView view)
        {
            throw new NotImplementedException();
        }
    }
}