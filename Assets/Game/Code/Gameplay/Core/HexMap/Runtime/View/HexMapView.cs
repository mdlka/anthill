using System;
using System.Collections.Generic;
using System.Linq;
using YellowSquad.HexMath;
using UnityEngine;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class HexMapView : MonoBehaviour, IHexMapView
    {
        private readonly Vector3 _hexRotation = new(0f, 30f, 0f);
        
        [SerializeField, Min(0.01f)] private Vector3 _hexScale;
        [SerializeField] private GroundMapView _groundView;
        [SerializeField] private PointsOfInterestView _pointsOfInterestView;
        [SerializeField] private UpdateLeafView _updateLeafView;
        [SerializeField] private SolidObjectView _closedHexesView;
        [SerializeField] private HexViewPair[] _hexViews;

        public bool Initialized { get; private set; }

        public void InitializeRender(float mapScale, IReadOnlyDictionary<AxialCoordinate, MapCell> cells, HashSet<AxialCoordinate> closedPositions)
        {
            if (Initialized)
                throw new InvalidOperationException();
            
            _groundView.Render(mapScale, _hexScale, cells.Keys);
            _pointsOfInterestView.InitializeRender(mapScale, cells);
            
            _closedHexesView.Clear();
            _closedHexesView.Render(closedPositions, position => HexMatrixBy(mapScale, position));
            
            foreach (var pair in cells)
            {
                if (closedPositions.Contains(pair.Key))
                    continue;
                
                var view = _hexViews.First(view => view.Hardness == pair.Value.Hex.Hardness).View;
                view.Render(pair.Value.Hex.Parts, Array.Empty<IReadOnlyPart>(), Array.Empty<IReadOnlyPart>(), HexMatrixBy(mapScale, pair.Key));
            }

            Initialized = true;
        }

        public void Render(float mapScale, HashSet<AxialCoordinate> closedPositions, params MapCellChange[] changes)
        {
            if (Initialized == false)
                throw new InvalidOperationException("Need initialize");
            
            _pointsOfInterestView.Render(mapScale, changes);
            _updateLeafView.Render(mapScale, changes);

            if (changes.Any(c => c.ChangeType == ChangeType.Hex && (c.RemovedParts.Any() || c.AddedParts.Any())))
            {
                _closedHexesView.Clear();
                _closedHexesView.Render(closedPositions, position => HexMatrixBy(mapScale, position));
            }
            
            foreach (var change in changes)
            {
                if (change.ChangeType != ChangeType.Hex)
                    continue;

                var view = _hexViews.First(view => view.Hardness == change.MapCell.Hex.Hardness).View;
                view.Render(change.AddedParts, change.RemovedParts, change.ChangedSizeParts, HexMatrixBy(mapScale, change.Position));
            }
        }

        public Matrix4x4 HexMatrixBy(float mapScale, AxialCoordinate position)
        {
            return Matrix4x4.TRS(position.ToVector3(mapScale), Quaternion.Euler(_hexRotation), _hexScale);
        }

        public Matrix4x4 PointOfInterestMatrixBy(float mapScale, AxialCoordinate position, PointOfInterestType type)
        {
            return _pointsOfInterestView.PointOfInterestMatrixBy(mapScale, position, type);
        }

        [Serializable]
        private class HexViewPair
        {
            [field: SerializeField] public Hardness Hardness { get; private set; }
            [field: SerializeField] public DividedObjectView View { get; private set; }
        }
    }
}