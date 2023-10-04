using System;
using System.Collections.Generic;
using YellowSquad.HexMath;

namespace YellowSquad.Core.HexMap
{
    public class Map : IHexMap
    {
        private readonly float _mapScale;
        private readonly Dictionary<AxialCoordinate, IHex> _hexes;

        public Map(float mapScale = 1f) : this(mapScale, new Dictionary<AxialCoordinate, IHex>()) { }
        
        public Map(float mapScale, IReadOnlyDictionary<AxialCoordinate, IHex> hexes)
        {
            if (mapScale <= 0)
                throw new ArgumentOutOfRangeException(nameof(mapScale));
            
            _mapScale = mapScale;
            _hexes = new Dictionary<AxialCoordinate, IHex>(hexes);
        }

        public bool HasHexIn(AxialCoordinate position)
        {
            return _hexes.ContainsKey(position);
        }

        public void AddHex(AxialCoordinate position, IHex hex)
        {
            if (_hexes.ContainsKey(position))
                throw new InvalidOperationException();
            
            _hexes.Add(position, hex);
        }

        public void RemoveHex(AxialCoordinate position)
        {
            if (_hexes.ContainsKey(position) == false)
                throw new InvalidOperationException();
            
            _hexes.Remove(position);
        }

        public void Visualize(IHexMapView view)
        {
            view.Render(_mapScale, _hexes.Keys);
        }
    }
}
