using System;
using System.Collections.Generic;
using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class SolidObjectView : MonoBehaviour
    {
        private readonly List<Matrix4x4> _matrices = new();
        private readonly List<List<Matrix4x4>> _otherMatrices = new();
        
        [SerializeField] private Mesh _mesh;
        [SerializeField] private Material _material;
        [SerializeField] private OtherInfo[] _other;

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

            for (int i = 0; i < _other.Length; i++)
                if (_otherMatrices[i].Count > 0)
                    Graphics.RenderMeshInstanced(_renderParams, _other[i].Mesh, 0, _otherMatrices[i]);
        }

        public void Render(IEnumerable<AxialCoordinate> positions, Func<AxialCoordinate, Matrix4x4> matrix)
        {
            foreach (var position in positions)
                _matrices.Add(matrix(position));

            foreach (var other in _other)
            {
                _otherMatrices.Add(new List<Matrix4x4>());

                foreach (var mainMatrix in _matrices)
                    _otherMatrices[^1].Add(mainMatrix * other.Matrix);
            }

            Rendered = true;
        }

        public void Clear()
        {
            _matrices.Clear();
            _otherMatrices.Clear();
            
            Rendered = false;
        }

        [Serializable]
        private class OtherInfo
        {
            private Matrix4x4? _matrix;
            
            [field: SerializeField] public Mesh Mesh { get; private set; }
            [field: SerializeField] public Vector3 LocalPosition { get; private set; }
            [field: SerializeField] public Vector3 Rotation { get; private set; }
            [field: SerializeField, Min(0f)] public float Size { get; private set; }
            
            public Matrix4x4 Matrix => _matrix ??= Matrix4x4.TRS(LocalPosition, Quaternion.Euler(Rotation), Vector3.one * Size);
        }
    }
}