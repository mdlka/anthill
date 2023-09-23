using System.Collections.Generic;
using YellowSquad.HexMath;

namespace YellowSquad.Core.HexMap
{
    public class Map : IHexMap
    {
        private readonly Dictionary<AxialCoordinate, IHex> _hexes;
        
        public Map(IReadOnlyDictionary<AxialCoordinate, IHex> hexes)
        {
            _hexes = new Dictionary<AxialCoordinate, IHex>(hexes);
        }

        public void RemoveHex(AxialCoordinate position)
        {
            _hexes.Remove(position);
        }

        public void Visualize(IHexMapView view)
        {
            view.Render(_hexes.Keys);
        }
    }
}
