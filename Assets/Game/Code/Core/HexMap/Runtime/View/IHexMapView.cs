using System.Collections.Generic;
using YellowSquad.HexMath;

namespace YellowSquad.Core.HexMap
{
    public interface IHexMapView
    {
        void Render(float mapScale, ICollection<AxialCoordinate> hexPositions);
    }
}