using System;
using System.Collections.Generic;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    public interface IHexMap
    {
        float Scale { get; }
        
        IEnumerable<AxialCoordinate> Positions { get; }

        bool HasPosition(AxialCoordinate position);
        bool HasObstacleIn(AxialCoordinate position);
        bool IsClosed(AxialCoordinate position);
        IHex HexFrom(AxialCoordinate position);

        bool HasDividedPointOfInterestIn(AxialCoordinate position);
        IDividedPointOfInterest DividedPointOfInterestFrom(AxialCoordinate position);
        PointOfInterestType PointOfInterestTypeIn(AxialCoordinate position);

        IReadOnlyList<AxialCoordinate> NeighborHexPositions(AxialCoordinate position, Func<AxialCoordinate, bool> where = null);
        IReadOnlyList<AxialCoordinate> PointsOfInterestPositions(PointOfInterestType targetPoint);

        void Visualize(IHexMapView view);
    }
}