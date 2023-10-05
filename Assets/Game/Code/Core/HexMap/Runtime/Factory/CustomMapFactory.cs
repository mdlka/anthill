using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    [CreateAssetMenu(menuName = "Anthill/Maps/Create HexMapFactory", fileName = "HexMapFactory", order = 56)]
    public class CustomMapFactory : BaseMapFactory
    {
        [SerializeField, Min(0.01f)] private float _mapScale = 0.57f;
        [SerializeField, HideInInspector] private List<HexWithPosition> _hexes;
        
        internal float MapScale => _mapScale;
        internal IEnumerable<HexWithPosition> Hexes => _hexes;

        public override IHexMap Create()
        {
            var hexes = _hexes.ToDictionary(pair => pair.Position, pair => pair.Hex);
            
            return new Map(_mapScale, hexes);
        }
        
        public override string ToString()
        {
            return string.Join(' ', _hexes.Select(pair => $"({pair.Hex}, {pair.Position.ToString()})"));
        }

        internal void AddHex(AxialCoordinate position, IHex hex)
        {
            if (_hexes.Any(pair => pair.Position == position))
                throw new InvalidOperationException();
            
            _hexes.Add(new HexWithPosition(position, hex));
        }

        internal void RemoveHex(AxialCoordinate position)
        {
            var targetHex = _hexes.FirstOrDefault(pair => pair.Position == position);
            
            if (targetHex == null)
                throw new InvalidOperationException();

            _hexes.Remove(targetHex);
        }

        [Serializable]
        internal class HexWithPosition
        {
            [SerializeField] private SerializedAxialCoordinate _position;
            [SerializeReference] private IHex _hex;
            
            public HexWithPosition(AxialCoordinate position, IHex hex)
            {
                _position = position;
                _hex = hex;
            }

            public IHex Hex => _hex;
            public AxialCoordinate Position => _position;
        }
    }
}