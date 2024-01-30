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
        [Header("Map settings")]
        [SerializeField, Min(0.01f)] private float _mapScale;
        [Header("Meshes")]
        [SerializeField] private HexMesh _emptyHexMesh;
        [SerializeField] private HexMesh _targetHexMesh;
        [SerializeField] private PointOfInterestMesh _targetLeafMesh;
        [Header("Editor settings")]
        [SerializeField] private bool _currentHexEmpty;
        [SerializeField] private Hardness _currentHexHardness;
        [SerializeField] private PointOfInterest _currentPointOfInterest;
        [SerializeField] private List<EditorMapHex> _hexes;
        
        internal float MapScale => _mapScale;
        internal IEnumerable<EditorMapHex> Hexes => _hexes;

        public override IHexMap Create()
        {
            var hexes = _hexes.ToDictionary(
                hex => hex.Position, 
                hex => new MapCell(
                    new Hex(hex.Hardness, hex.Empty ? _emptyHexMesh : _targetHexMesh), 
                    hex.PointOfInterest));
            
            return new Map(_mapScale, hexes);
        }
        
        public override string ToString()
        {
            return string.Join(' ', _hexes.Select(pair => $"({pair.Position.ToString()})"));
        }

        internal bool HasHexIn(AxialCoordinate position)
        {
            return _hexes.Any(pair => pair.Position == position);
        }

        internal void AddHex(AxialCoordinate position)
        {
            if (HasHexIn(position))
                throw new InvalidOperationException();
            
            _hexes.Add(new EditorMapHex(position, _currentHexHardness, _currentPointOfInterest, _currentHexEmpty));
        }

        internal void RemoveHex(AxialCoordinate position)
        {
            var targetHex = _hexes.FirstOrDefault(pair => pair.Position == position);
            
            if (targetHex == null)
                throw new InvalidOperationException();

            _hexes.Remove(targetHex);
        }
        
        [Serializable]
        internal class EditorMapHex
        {
            [SerializeField] private bool _empty;
            [SerializeField] private Hardness _hardness;
            [SerializeField] private PointOfInterest _pointOfInterest;
            [SerializeField] private SerializedAxialCoordinate _position;

            public EditorMapHex(AxialCoordinate position, Hardness hardness, PointOfInterest pointOfInterest, bool empty)
            {
                _position = position;
                _hardness = hardness;
                _pointOfInterest = pointOfInterest;
                _empty = empty;
            }

            public bool Empty => _empty;
            public Hardness Hardness => _hardness;
            public PointOfInterest PointOfInterest => _pointOfInterest;
            public AxialCoordinate Position => _position;
        }
    }
}