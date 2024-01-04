using System.Collections.Generic;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    public interface IHexMap
    {
        float Scale { get; }
        
        bool HasPosition(AxialCoordinate position);
        bool HasObstacleIn(AxialCoordinate position);
        IHex HexFrom(AxialCoordinate position);

        IReadOnlyList<AxialCoordinate> NeighborHexPosition(AxialCoordinate position);
        IReadOnlyList<AxialCoordinate> PointsOfInterestPositions(PointOfInterest targetPoint);

        void Visualize(IHexMapView view);
    }
}