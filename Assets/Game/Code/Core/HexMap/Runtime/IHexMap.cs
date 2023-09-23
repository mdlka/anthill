using YellowSquad.HexMath;

namespace YellowSquad.Core.HexMap
{
    public interface IHexMap
    {
        void RemoveHex(AxialCoordinate position);
        void Visualize(IHexMapView view);
    }
}