using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using YellowSquad.GamePlatformSdk;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    [CreateAssetMenu(menuName = "Anthill/Maps/Create HexMapFactory", fileName = "HexMapFactory", order = 56)]
    public class CustomMapFactory : BaseMapFactory
    {
        [Header("Map settings")]
        [SerializeField, Min(0.01f)] private float _mapScale = 1f;
        [Header("Hexes")]
        [SerializeField] private HexMesh _emptyHexMesh;
        [SerializeField] private HexMesh _targetHexMesh;
        [Header("Points of interest")]
        [SerializeField] private PointOfInterestMesh _targetLeafMesh;
        [SerializeField] private Hardness _targetLeafHardness;
        [Header("Editor settings")] 
        [SerializeField, Min(0.0001f)] private float _textScaleFactor = 1f;
        [SerializeField] private bool _currentHexEmpty;
        [SerializeField] private Hardness _currentHexHardness;
        [SerializeField] private PointOfInterestType _currentPointOfInterest;
        [SerializeField] private List<EditorMapHex> _hexes;

        internal float MapScale => _mapScale;
        internal float TextScaleFactor => _textScaleFactor;
        internal IEnumerable<EditorMapHex> Hexes => _hexes;

        public override IHexMap Create(ISave save)
        {
            var openedPositions = new HashSet<AxialCoordinate>();
            var mapSave = new MapSave();

            if (save.HasKey(SaveConstants.MapSaveKey))
            {
                mapSave = JsonConvert.DeserializeObject<MapSave>(save.GetString(SaveConstants.MapSaveKey));

                foreach (var position in mapSave.OpenPositions)
                    openedPositions.Add(new AxialCoordinate(position.Q, position.R));
            }
            
            var hexes = _hexes.ToDictionary(
                hex => hex.Position, 
                hex => new MapCell(
                    new Hex(hex.Hardness, hex.Empty || openedPositions.Contains(hex.Position) ? _emptyHexMesh : _targetHexMesh),
                    hex.PointOfInterestType,
                    DividedPointOfInterestBy(hex.PointOfInterestType)));
            
            return new Map(_mapScale, hexes, save, mapSave);
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

        private IDividedPointOfInterest DividedPointOfInterestBy(PointOfInterestType type)
        {
            return type switch
            {
                PointOfInterestType.Leaf => new Leaf(_targetLeafHardness, _targetLeafMesh),
                _ => null
            };
        }

#if UNITY_EDITOR
        [ContextMenu(nameof(MoveUp))]
        private void MoveUp()
        {
            MoveHexes(new AxialCoordinate(0, 1));
        }

        [ContextMenu(nameof(MoveDown))]
        private void MoveDown()
        {
            MoveHexes(new AxialCoordinate(0, -1));
        }

        [ContextMenu(nameof(SetNotEmptyAllHex))]
        private void SetNotEmptyAllHex()
        {
            var hexes = new List<EditorMapHex>();

            foreach (var hex in _hexes)
                hexes.Add(new EditorMapHex(hex.Position, hex.Hardness, hex.PointOfInterestType, empty: false));

            _hexes = hexes;
            EditorUtility.SetDirty(this);
        }

        [ContextMenu(nameof(SetCurrentHardnessForAllHex))]
        private void SetCurrentHardnessForAllHex()
        {
            var hexes = new List<EditorMapHex>();

            foreach (var hex in _hexes)
                hexes.Add(new EditorMapHex(hex.Position, _currentHexHardness, hex.PointOfInterestType, hex.Empty));

            _hexes = hexes;
            EditorUtility.SetDirty(this);
        }
        
        private void MoveHexes(AxialCoordinate distance)
        {
            var hexes = new List<EditorMapHex>();

            foreach (var hex in _hexes)
                hexes.Add(new EditorMapHex(hex.Position + distance, hex.Hardness, hex.PointOfInterestType, hex.Empty));

            _hexes = hexes;
            EditorUtility.SetDirty(this);
        }
#endif
        
        [Serializable]
        internal class EditorMapHex
        {
            [SerializeField] private bool _empty;
            [SerializeField] private Hardness _hardness;
            [SerializeField] private PointOfInterestType _pointOfInterestType;
            [SerializeField] private SerializedAxialCoordinate _position;

            public EditorMapHex(AxialCoordinate position, Hardness hardness, PointOfInterestType pointOfInterestType, bool empty)
            {
                _position = position;
                _hardness = hardness;
                _pointOfInterestType = pointOfInterestType;
                _empty = empty;
            }

            public bool Empty => _empty;
            public Hardness Hardness => _hardness;
            public PointOfInterestType PointOfInterestType => _pointOfInterestType;
            public AxialCoordinate Position => _position;
        }
    }
}