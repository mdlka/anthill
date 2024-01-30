using System.Collections.Generic;
using UnityEngine;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class DividedObjectView : MonoBehaviour
    {
        private readonly Dictionary<Mesh, List<Matrix4x4>> _partMatrices = new();

        [SerializeField] private Material _targetMaterial;
        [SerializeField] private BaseDividedObjectMesh _targetMesh;
        
        private RenderParams _renderParams;
        private IReadOnlyDictionary<Vector3, Mesh> _meshByPartPosition;
        
        private void Awake()
        {
            _renderParams = new RenderParams(_targetMaterial);
            _meshByPartPosition = _targetMesh.PartsMeshesByLocalPosition;
        }
        
        private void Update()
        {
            if (_partMatrices.Count == 0)
                return;

            foreach (var renderPart in _partMatrices)
                Graphics.RenderMeshInstanced(_renderParams, renderPart.Key, 0, renderPart.Value);
        }
        
        public void Render(IEnumerable<IReadOnlyPart> parts, Matrix4x4 objectMatrix)
        {
            foreach (var part in parts)
            {
                if (part.Destroyed)
                    continue;
                
                var mesh = _meshByPartPosition[part.LocalPosition];
                
                if (_partMatrices.ContainsKey(mesh) == false)
                    _partMatrices.Add(mesh, new List<Matrix4x4>());

                var partMatrix = objectMatrix * PartMatrixBy(part.LocalPosition);
                
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