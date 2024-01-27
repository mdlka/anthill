using System.Collections.Generic;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    public interface IHexMapView
    {
        void Render(float mapScale, IReadOnlyDictionary<AxialCoordinate, MapCell> cells, HashSet<AxialCoordinate> closedPosition);
    }
}