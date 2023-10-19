using System;
using System.Collections.Generic;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class Map : IHexMap
    {
        private readonly float _scale;
        private readonly Dictionary<AxialCoordinate, IHex> _hexes;

        public Map(float scale = 1f) : this(scale, new Dictionary<AxialCoordinate, IHex>()) { }
        
        public Map(float scale, IReadOnlyDictionary<AxialCoordinate, IHex> hexes)
        {
            if (scale <= 0)
                throw new ArgumentOutOfRangeException(nameof(scale));
            
            _scale = scale;
            _hexes = new Dictionary<AxialCoordinate, IHex>(hexes);
        }

        public float Scale => _scale;

        public bool HasHexIn(AxialCoordinate position)
        {
            return _hexes.ContainsKey(position);
        }

        public IHex HexFrom(AxialCoordinate position)
        {
            if (HasHexIn(position) == false)
                throw new InvalidOperationException();
            
            return _hexes[position];
        }

        public void AddHex(AxialCoordinate position, IHex hex)
        {
            if (_hexes.TryAdd(position, hex) == false)
                throw new InvalidOperationException();
        }

        public void RemoveHex(AxialCoordinate position)
        {
            if (_hexes.Remove(position) == false)
                throw new InvalidOperationException();
        }

        public void Visualize(IHexMapView view)
        {
            view.Render(_scale, _hexes);
        }

        public override string ToString()
        {
            return string.Join(' ', _hexes);
        }
    }
}
