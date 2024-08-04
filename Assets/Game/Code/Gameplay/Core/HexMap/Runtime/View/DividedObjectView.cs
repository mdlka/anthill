using System.Collections.Generic;
using UnityEngine;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class DividedObjectView : MonoBehaviour
    {
        private readonly Dictionary<Mesh, List<Matrix4x4>> _partMatrices = new();
        private readonly Dictionary<Mesh, List<IReadOnlyPart>> _meshToPart = new();

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
                if (renderPart.Value.Count > 0)
                    Graphics.RenderMeshInstanced(_renderParams, renderPart.Key, 0, renderPart.Value);
        }
        
        public void Render(IEnumerable<IReadOnlyPart> addedParts, IEnumerable<IReadOnlyPart> removedParts, 
            IEnumerable<IReadOnlyPart> changedSizeParts, Matrix4x4 objectMatrix)
        {
            foreach (var part in changedSizeParts)
            {
                var mesh = _meshByPartPosition[part.LocalPosition];

                if (_partMatrices.ContainsKey(mesh) == false) 
                    continue;
                
                int partIndex = _meshToPart[mesh].FindIndex(p => p == part);
                
                if (partIndex < 0)
                    continue;
                
                _partMatrices[mesh][partIndex] = objectMatrix * PartMatrixBy(part.LocalPosition, part.Size);
            }
            
            foreach (var part in removedParts)
            {
                var mesh = _meshByPartPosition[part.LocalPosition];

                if (_partMatrices.ContainsKey(mesh) == false) 
                    continue;
                
                int partIndex = _meshToPart[mesh].FindIndex(p => p == part);
                _meshToPart[mesh].RemoveAt(partIndex);
                _partMatrices[mesh].RemoveAt(partIndex);
            }
            
            foreach (var part in addedParts)
            {
                var mesh = _meshByPartPosition[part.LocalPosition];

                if (_partMatrices.ContainsKey(mesh) == false)
                {
                    _partMatrices.Add(mesh, new List<Matrix4x4>());
                    _meshToPart.Add(mesh, new List<IReadOnlyPart>());
                }
                
                _partMatrices[mesh].Add(objectMatrix * PartMatrixBy(part.LocalPosition));
                _meshToPart[mesh].Add(part);
            }
        }

        private Matrix4x4 PartMatrixBy(Vector3 worldPosition, float sizeFactor = 1f)
        {
            return Matrix4x4.TRS(worldPosition, Quaternion.Euler(-90f, 0f, 0f), Vector3.one * sizeFactor);
        }
    }
}