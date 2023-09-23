using System.Collections.Generic;
using YellowSquad.HexMath;

namespace YellowSquad.Core.HexMap
{
    public class Map : IHexMap
    {
        private readonly float _mapScale;
        private readonly Dictionary<AxialCoordinate, IHex> _hexes;
        
        public Map(float mapScale, IReadOnlyDictionary<AxialCoordinate, IHex> hexes)
        {
            _mapScale = mapScale;
            _hexes = new Dictionary<AxialCoordinate, IHex>(hexes);
        }

        public void RemoveHex(AxialCoordinate position)
        {
            _hexes.Remove(position);
        }

        public void Visualize(IHexMapView view)
        {
            view.Render(_mapScale, _hexes.Keys);
        }
    }
}
