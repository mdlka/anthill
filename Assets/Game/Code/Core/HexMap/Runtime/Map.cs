using System;
using System.Collections.Generic;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class Map : IHexMap
    {
        private readonly float _scale;
        private readonly Dictionary<AxialCoordinate, IHex> _hexes;

        public Map(IReadOnlyDictionary<AxialCoordinate, IHex> hexes) : this(1f, hexes) { }
        
        public Map(float scale, IReadOnlyDictionary<AxialCoordinate, IHex> hexes)
        {
            if (scale <= 0)
                throw new ArgumentOutOfRangeException(nameof(scale));
            
            _scale = scale;
            _hexes = new Dictionary<AxialCoordinate, IHex>(hexes);
        }

        public float Scale => _scale;

        public bool HasPosition(AxialCoordinate position)
        {
            return _hexes.ContainsKey(position);
        }

        public bool HasObstacleIn(AxialCoordinate position)
        {
            if (HasPosition(position) == false)
                throw new ArgumentOutOfRangeException();

            return _hexes[position].HasParts;
        }

        public IHex HexFrom(AxialCoordinate position)
        {
            if (HasPosition(position) == false)
                throw new ArgumentOutOfRangeException();
            
            return _hexes[position];
        }

        public void Visualize(IHexMapView view)
        {
            view.Render(_scale, _hexes);
        }

        public override string ToString()
        {
            return $"Count: {_hexes.Count}\n{string.Join(' ', _hexes)}";
        }
    }
}
