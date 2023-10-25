using System.Collections.Generic;
using YellowSquad.HexMath;
using UnityEngine;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class HexMapView : MonoBehaviour, IHexMapView
    {
        private readonly List<Matrix4x4> _groundHexMatrices = new();
        
        [SerializeField, Min(0.01f)] private Vector3 _hexScale;
        [SerializeField] private HexView _hexView;

        [Header("Ground Render Settings")]
        [SerializeField, Min(0.01f)] private Vector3 _groundHexScale;
        [SerializeField] private Mesh _groundHexMesh;
        [SerializeField] private Material _groundHexMaterial;

        private RenderParams _groundRenderParams;
        private bool _groundHexInitialized;

        private void Awake()
        {
            _groundRenderParams = new RenderParams(_groundHexMaterial);
        }

        private void Update()
        {
            if (_groundHexMatrices.Count == 0)
                return;

            Graphics.RenderMeshInstanced(_groundRenderParams, _groundHexMesh, 0, _groundHexMatrices);
        }

        public void Render(float mapScale, IReadOnlyDictionary<AxialCoordinate, IHex> hexes)
        {
            _hexView.Clear();
            
            foreach (var pair in hexes)
            {
                var hexMatrix = Matrix4x4.TRS(pair.Key.ToVector3(mapScale), Quaternion.Euler(0f, 30f, 0f), _hexScale);
                _hexView.Render(pair.Value.Parts, hexMatrix);
                
                if (_groundHexInitialized == false)
                    _groundHexMatrices.Add(Matrix4x4.TRS(pair.Key.ToVector3(mapScale) - Vector3.up * _groundHexMesh.bounds.size.y, Quaternion.Euler(0f, 30f, 0f), _groundHexScale));
            }

            _groundHexInitialized = true;
        }
    }
}