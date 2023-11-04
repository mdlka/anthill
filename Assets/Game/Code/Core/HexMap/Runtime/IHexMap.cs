using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    public interface IHexMap
    {
        float Scale { get; }
        
        bool HasPosition(AxialCoordinate position);
        bool HasObstacleIn(AxialCoordinate position);
        IHex HexFrom(AxialCoordinate position);
        
        void Visualize(IHexMapView view);
    }
}