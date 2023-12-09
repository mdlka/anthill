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
        [SerializeField] private HexViewPair[] _hexViews;

        public void Render(float mapScale, IReadOnlyDictionary<AxialCoordinate, MapCell> cells)
        {
            if (_groundView.Initialized == false)
                _groundView.Initialize(mapScale, _hexScale, cells.Keys);
            
            foreach (var pair in _hexViews)
                pair.View.Clear(); // TODO: Need optimization
            
            foreach (var pair in cells)
            {
                var hexMatrix = Matrix4x4.TRS(pair.Key.ToVector3(mapScale), Quaternion.Euler(0f, 30f, 0f), _hexScale);

                var view = _hexViews.First(view => view.Hardness == pair.Value.Hex.Hardness).View;
                view.Render(pair.Value.Hex.Parts, hexMatrix);
            }
        }

        [Serializable]
        private class HexViewPair
        {
            [field: SerializeField] public Hardness Hardness { get; private set; }
            [field: SerializeField] public HexView View { get; private set; }
        }
    }
}