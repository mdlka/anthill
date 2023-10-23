﻿using System.Collections.Generic;
using UnityEngine;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class HexView : MonoBehaviour
    {
        private readonly Dictionary<Mesh, List<Matrix4x4>> _partMatrices = new();

        [SerializeField] private Material _hexMaterial;
        [SerializeField] private HexMesh _targetHexMesh;
        
        private RenderParams _renderParams;
        private IReadOnlyDictionary<Vector3, Mesh> _meshByPartPosition;
        
        private void Awake()
        {
            _renderParams = new RenderParams(_hexMaterial);
            _meshByPartPosition = _targetHexMesh.MeshByPartLocalPosition;
        }
        
        private void Update()
        {
            if (_partMatrices.Count == 0)
                return;

            foreach (var renderPart in _partMatrices)
                Graphics.RenderMeshInstanced(_renderParams, renderPart.Key, 0, renderPart.Value);
        }

        public void Render(IReadOnlyCollection<IReadOnlyHexPart> parts, Matrix4x4 hexMatrix)
        {
            foreach (var part in parts)
            {
                if (part.NeedRender == false)
                    continue;
                
                var mesh = _meshByPartPosition[part.LocalPosition];
                
                if (_partMatrices.ContainsKey(mesh) == false)
                    _partMatrices.Add(mesh, new List<Matrix4x4>());

                var partMatrix = hexMatrix * PartMatrixBy(part.LocalPosition);
                
                _partMatrices[mesh].Add(partMatrix);
            }
        }

        public void Clear()
        {
            _partMatrices.Clear();
        }
        
        private Matrix4x4 PartMatrixBy(Vector3 worldPosition)
        {
            return Matrix4x4.TRS(worldPosition, Quaternion.Euler(-90f, 0f, 0f), Vector3.one);
        }
    }
}