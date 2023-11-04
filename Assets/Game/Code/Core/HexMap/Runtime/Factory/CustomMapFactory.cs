using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YellowSquad.HexMath;
using Random = UnityEngine.Random;

namespace YellowSquad.Anthill.Core.HexMap
{
    [CreateAssetMenu(menuName = "Anthill/Maps/Create HexMapFactory", fileName = "HexMapFactory", order = 56)]
    public class CustomMapFactory : BaseMapFactory
    {
        [SerializeField, Min(0.01f)] private float _mapScale;
        [SerializeField] private HexMesh _hexMesh;
        [SerializeField, HideInInspector] private List<HexWithPosition> _hexes;
        
        internal float MapScale => _mapScale;
        internal IEnumerable<HexWithPosition> Hexes => _hexes;

        public override IHexMap Create()
        {
            // TODO: Need to remove manual create of hex via code
            var hexes = _hexes.ToDictionary(pair => pair.Position, _ => (IHex)new DefaultHex((Hardness)Random.Range(0, 3), _hexMesh));
            
            return new Map(_mapScale, hexes);
        }
        
        public override string ToString()
        {
            return string.Join(' ', _hexes.Select(pair => $"({pair.Position.ToString()})"));
        }

        internal void AddHex(AxialCoordinate position, IHex hex)
        {
            if (_hexes.Any(pair => pair.Position == position))
                throw new InvalidOperationException();
            
            _hexes.Add(new HexWithPosition(position));
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
            
            public HexWithPosition(AxialCoordinate position)
            {
                _position = position;
            }

            public AxialCoordinate Position => _position;
        }
    }
}