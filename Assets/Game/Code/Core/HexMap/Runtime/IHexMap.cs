using YellowSquad.HexMath;

namespace YellowSquad.Core.HexMap
{
    public interface IHexMap
    {
        float Scale { get; }
        
        bool HasHexIn(AxialCoordinate position);
        
        void AddHex(AxialCoordinate position, IHex hex);
        void RemoveHex(AxialCoordinate position);
        
        void Visualize(IHexMapView view);
    }
}