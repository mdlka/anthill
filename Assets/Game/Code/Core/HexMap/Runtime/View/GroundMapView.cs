using System;
using System.Collections.Generic;
using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class GroundMapView : MonoBehaviour
    {
        private readonly List<Matrix4x4> _groundHexMatrices = new();

        [SerializeField] private Mesh _hexMesh;
        [SerializeField] private Material _hexMaterial;

        private RenderParams _groundRenderParams;
        
        public bool Initialized { get; private set; }
        
        private void Awake()
        {
            _groundRenderParams = new RenderParams(_hexMaterial);
        }

        private void Update()
        {
            if (_groundHexMatrices.Count == 0)
                return;

            Graphics.RenderMeshInstanced(_groundRenderParams, _hexMesh, 0, _groundHexMatrices);
        }

        public void Initialize(float mapScale, Vector3 hexScale, IEnumerable<AxialCoordinate> hexPositions)
        {
            if (Initialized)
                throw new InvalidOperationException("Already initialized");
            
            Vector3 offsetY = Vector3.up * _hexMesh.bounds.size.y;
            Vector3 groundHexScale = new Vector3(hexScale.x, 1f, hexScale.z);
            
            foreach (var position in hexPositions)
                _groundHexMatrices.Add(Matrix4x4.TRS(position.ToVector3(mapScale) - offsetY, Quaternion.Euler(0f, 30f, 0f), groundHexScale));

            Initialized = true;
        }
    }
}