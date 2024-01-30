using System;
using System.Collections.Generic;
using System.Linq;
using YellowSquad.HexMath;
using UnityEngine;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class HexMapView : MonoBehaviour, IHexMapView
    {
        [SerializeField, Min(0.01f)] private Vector3 _hexScale;
        [SerializeField] private GroundMapView _groundView;
        [SerializeField] private PointsOfInterestView _pointsOfInterestView;
        [SerializeField] private DividedObjectView _leafView;
        [SerializeField] private SolidHexView _closedHexesView;
        [SerializeField] private HexViewPair[] _hexViews;

        public void Render(float mapScale, IReadOnlyDictionary<AxialCoordinate, MapCell> cells, HashSet<AxialCoordinate> closedPosition)
        {
            if (_groundView.Rendered == false)
                _groundView.Render(mapScale, _hexScale, cells.Keys);
            
            if (_pointsOfInterestView.Rendered == false)
                _pointsOfInterestView.Render(mapScale, cells.ToDictionary(pair => pair.Key, pair => pair.Value.PointOfInterestType));
            
            _closedHexesView.Clear();
            _closedHexesView.Render(mapScale, _hexScale, closedPosition);
            
            foreach (var pair in _hexViews)
                pair.View.Clear(); // TODO: Need optimization

            foreach (var pair in cells)
            {
                if (closedPosition.Contains(pair.Key))
                    continue;
                
                var hexMatrix = HexMatrixBy(mapScale, pair.Key);
                var view = _hexViews.First(view => view.Hardness == pair.Value.Hex.Hardness).View;
                
                if (pair.Value.HasDividedPointOfInterest)
                    _leafView.Render(pair.Value.DividedPointOfInterest.Parts, PointOfInterestMatrixBy(mapScale, pair.Key, pair.Value.PointOfInterestType));
                
                view.Render(pair.Value.Hex.Parts, hexMatrix);
            }
        }

        public Matrix4x4 HexMatrixBy(float mapScale, AxialCoordinate position)
        {
            return Matrix4x4.TRS(position.ToVector3(mapScale), Quaternion.Euler(0f, 30f, 0f), _hexScale);
        }

        public Matrix4x4 PointOfInterestMatrixBy(float mapScale, AxialCoordinate position, PointOfInterestType type)
        {
            return Matrix4x4.TRS(position.ToVector3(mapScale), Quaternion.Euler(0f, 30f, 0f), Vector3.one);
        }

        [Serializable]
        private class HexViewPair
        {
            [field: SerializeField] public Hardness Hardness { get; private set; }
            [field: SerializeField] public DividedObjectView View { get; private set; }
        }
    }
}