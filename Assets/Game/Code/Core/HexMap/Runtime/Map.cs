using System;
using System.Collections.Generic;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class Map : IHexMap
    {
        private readonly float _scale;
        private readonly Dictionary<AxialCoordinate, MapCell> _cells;

        public Map(IReadOnlyDictionary<AxialCoordinate, MapCell> cells) : this(1f, cells) { }
        
        public Map(float scale, IReadOnlyDictionary<AxialCoordinate, MapCell> cells)
        {
            if (scale <= 0)
                throw new ArgumentOutOfRangeException(nameof(scale));
            
            _scale = scale;
            _cells = new Dictionary<AxialCoordinate, MapCell>(cells);
        }

        public float Scale => _scale;

        public bool HasPosition(AxialCoordinate position)
        {
            return _cells.ContainsKey(position);
        }

        public bool HasObstacleIn(AxialCoordinate position)
        {
            if (HasPosition(position) == false)
                throw new ArgumentOutOfRangeException();

            return _cells[position].Hex.HasParts;
        }

        public IHex HexFrom(AxialCoordinate position)
        {
            if (HasPosition(position) == false)
                throw new ArgumentOutOfRangeException();
            
            return _cells[position].Hex;
        }

        public void Visualize(IHexMapView view)
        {
            view.Render(_scale, _cells);
        }

        public override string ToString()
        {
            return $"Count: {_cells.Count}\n{string.Join(' ', _cells)}";
        }
    }
}
