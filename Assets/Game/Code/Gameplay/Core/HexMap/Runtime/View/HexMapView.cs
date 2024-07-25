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

        public void Render(float mapScale, IReadOnlyDictionary<AxialCoordinate, MapCell> cells, HashSet<AxialCoordinate> closedPositions)
        {
            if (_groundView.Rendered == false)
                _groundView.Render(mapScale, _hexScale, cells.Keys);
            
            _pointsOfInterestView.Render(mapScale, cells);
            _updateLeafView.Render(mapScale, cells);
            
            _closedHexesView.Clear();
            _closedHexesView.Render(closedPositions, position => HexMatrixBy(mapScale, position));
            
            foreach (var pair in _hexViews)
                pair.View.Clear(); // TODO: Need optimization

            foreach (var pair in cells)
            {
                if (closedPositions.Contains(pair.Key))
                    continue;
                
                var view = _hexViews.First(view => view.Hardness == pair.Value.Hex.Hardness).View;
                view.Render(pair.Value.Hex.Parts, HexMatrixBy(mapScale, pair.Key));
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