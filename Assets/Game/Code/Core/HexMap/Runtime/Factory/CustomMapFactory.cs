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
        [SerializeField, Min(0.01f)] private float _mapScale;
        
        [Header("Editor settings")]
        [SerializeField] private HexMesh _currentTargetHexMesh;
        [SerializeField] private Hardness _currentHexHardness;
        [SerializeField] private List<HexWithPosition> _hexes;
        
        internal float MapScale => _mapScale;
        internal IEnumerable<HexWithPosition> Hexes => _hexes;

        public override IHexMap Create()
        {
            var hexes = _hexes.ToDictionary(hex => hex.Position, hex => (IHex)new DefaultHex(hex.Hardness, hex.TargetHexMesh));
            
            return new Map(_mapScale, hexes);
        }
        
        public override string ToString()
        {
            return string.Join(' ', _hexes.Select(pair => $"({pair.Position.ToString()})"));
        }

        internal void AddHex(AxialCoordinate position)
        {
            if (_hexes.Any(pair => pair.Position == position))
                throw new InvalidOperationException();
            
            _hexes.Add(new HexWithPosition(_currentHexHardness, _currentTargetHexMesh, position));
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
            [SerializeField] private Hardness _hardness;
            [SerializeField] private HexMesh _targetHexMesh;
            [SerializeField] private SerializedAxialCoordinate _position;

            public HexWithPosition(Hardness hardness, HexMesh targetHexMesh, AxialCoordinate position)
            {
                _hardness = hardness;
                _targetHexMesh = targetHexMesh;
                _position = position;
            }

            public Hardness Hardness => _hardness;
            public IHexMesh TargetHexMesh => _targetHexMesh;
            public AxialCoordinate Position => _position;
        }
    }
}