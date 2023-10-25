using System;
using System.Collections.Generic;
using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class GroundMapView : MonoBehaviour
    {
        private readonly List<Matrix4x4> _groundHexMatrices = new();

        [SerializeField, Min(0.01f)] private Vector3 _groundHexScale;
        [SerializeField] private Mesh _groundHexMesh;
        [SerializeField] private Material _groundHexMaterial;

        private RenderParams _groundRenderParams;
        
        public bool Initialized { get; private set; }
        
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

        public void Initialize(float mapScale, IEnumerable<AxialCoordinate> hexPositions)
        {
            if (Initialized)
                throw new InvalidOperationException("Already initialized");
            
            var offsetY = Vector3.up * _groundHexMesh.bounds.size.y;
            
            foreach (var position in hexPositions)
                _groundHexMatrices.Add(Matrix4x4.TRS(position.ToVector3(mapScale) - offsetY, Quaternion.Euler(0f, 30f, 0f), _groundHexScale));

            Initialized = true;
        }
    }
}