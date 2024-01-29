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
        [SerializeField] private DividedObjectMesh _currentTargetHexMesh;
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
                    new Hex(hex.Hardness, hex.TargetHexMesh), 
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
            
            _hexes.Add(new EditorMapHex(_currentPointOfInterest, _currentHexHardness, _currentTargetHexMesh, position));
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
            [SerializeField] private PointOfInterest _pointOfInterest;
            [SerializeField] private Hardness _hardness;
            [SerializeField] private DividedObjectMesh _targetHexMesh;
            [SerializeField] private SerializedAxialCoordinate _position;

            public EditorMapHex(PointOfInterest pointOfInterest, Hardness hardness, DividedObjectMesh targetHexMesh, AxialCoordinate position)
            {
                _pointOfInterest = pointOfInterest;
                _hardness = hardness;
                _targetHexMesh = targetHexMesh;
                _position = position;
            }

            public PointOfInterest PointOfInterest => _pointOfInterest;
            public Hardness Hardness => _hardness;
            public IDividedObjectMesh TargetHexMesh => _targetHexMesh;
            public AxialCoordinate Position => _position;
            internal string TargetHexMeshName => _targetHexMesh.name;
        }
    }
}