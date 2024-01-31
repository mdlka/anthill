using System;
using System.Collections.Generic;
using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class SolidObjectView : MonoBehaviour
    {
        private readonly List<Matrix4x4> _matrices = new();

        [SerializeField] private Mesh _mesh;
        [SerializeField] private Material _material;

        private RenderParams _renderParams;
        
        public bool Rendered { get; private set; }
        
        private void Awake()
        {
            _renderParams = new RenderParams(_material);
        }

        private void Update()
        {
            if (_matrices.Count == 0)
                return;

            Graphics.RenderMeshInstanced(_renderParams, _mesh, 0, _matrices);
        }

        public void Render(IEnumerable<AxialCoordinate> positions, Func<AxialCoordinate, Matrix4x4> matrix)
        {
            foreach (var position in positions)
                _matrices.Add(matrix(position));

            Rendered = true;
        }

        public void Clear()
        {
            _matrices.Clear();
            Rendered = false;
        }
    }
}