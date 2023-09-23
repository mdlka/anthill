using System.Collections.Generic;
using YellowSquad.HexMath;
using UnityEngine;

namespace YellowSquad.Core.HexMap
{
    internal class HexMapView : MonoBehaviour, IHexMapView
    {
        private readonly Dictionary<AxialCoordinate, Matrix4x4> _matricesByPosition = new();
        private readonly List<AxialCoordinate> _lastRenderedPositions = new();
        private readonly List<Matrix4x4> _matrices = new();

        [SerializeField] private Mesh _hexMesh;
        [SerializeField] private Material _hexMaterial;

        private bool _hasChanges;
        private RenderParams _renderParams;

        private void Awake()
        {
            _renderParams = new RenderParams(_hexMaterial);
        }

        private void Update()
        {
            Graphics.RenderMeshInstanced(_renderParams, _hexMesh, 0, _matrices);
        }

        public void Render(ICollection<AxialCoordinate> hexPositions)
        {
            RemoveExtraPositions(hexPositions);
            AddNewPositions(hexPositions);

            if (_hasChanges == false)
                return;
            
            _matrices.Clear();
            _matrices.AddRange(_matricesByPosition.Values);
        }
        
        private void RemoveExtraPositions(ICollection<AxialCoordinate> hexPositions)
        {
            for (int i = _lastRenderedPositions.Count - 1; i >= 0; i--)
            {
                var position = _lastRenderedPositions[i];
                
                if (_lastRenderedPositions.Contains(position))
                    continue;

                _lastRenderedPositions.Remove(position);
                _matricesByPosition.Remove(position);

                _hasChanges = true;
            }
        }

        private void AddNewPositions(ICollection<AxialCoordinate> hexPositions)
        {
            foreach (var position in hexPositions)
            {
                if (_lastRenderedPositions.Contains(position))
                    continue;
                
                _lastRenderedPositions.Add(position);
                _matricesByPosition.Add(position, MatrixBy(position.ToVector3()));

                _hasChanges = true;
            }
        }

        private Matrix4x4 MatrixBy(Vector3 worldPosition)
        {
            return Matrix4x4.TRS(worldPosition, Quaternion.identity, Vector3.one);
        }
    }
}