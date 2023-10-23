using System.Collections.Generic;
using YellowSquad.HexMath;
using UnityEngine;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class HexMapView : MonoBehaviour, IHexMapView
    {
        [SerializeField, Min(0.01f)] private Vector3 _hexScale;
        [SerializeField] private HexView _hexView;

        public void Render(float mapScale, IReadOnlyDictionary<AxialCoordinate, IHex> hexes)
        {
            _hexView.Clear();
            
            foreach (var pair in hexes)
            {
                var hexMatrix = Matrix4x4.TRS(pair.Key.ToVector3(mapScale), Quaternion.Euler(0f, 30f, 0f), _hexScale);
                _hexView.Render(pair.Value.Parts, hexMatrix);
            }
        }
    }
}