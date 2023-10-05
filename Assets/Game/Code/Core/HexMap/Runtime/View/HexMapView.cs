using System.Collections.Generic;
using YellowSquad.HexMath;
using UnityEngine;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class HexMapView : MonoBehaviour, IHexMapView
    {
        private readonly Dictionary<AxialCoordinate, Matrix4x4> _matricesByPosition = new();
        private readonly List<AxialCoordinate> _lastRenderedPositions = new();
        private readonly List<Matrix4x4> _matrices = new();

        [SerializeField] private Mesh _hexMesh;
        [SerializeField] private Material _hexMaterial;
        [SerializeField, Min(0.01f)] private Vector3 _hexScale;

        private bool _hasChanges;
        private RenderParams _renderParams;

        private void Awake()
        {
            _renderParams = new RenderParams(_hexMaterial);
        }

        private void Update()
        {
            if (_matrices.Count == 0)
                return;

            Graphics.RenderMeshInstanced(_renderParams, _hexMesh, 0, _matrices);
        }

        public void Render(float mapScale, ICollection<AxialCoordinate> hexPositions)
        {
            RemoveExtraPositions(hexPositions);
            AddNewPositions(mapScale, hexPositions);

            if (_hasChanges == false)
                return;
            
            _matrices.Clear();
            _matrices.AddRange(_matricesByPosition.Values);
            _hasChanges = false;
        }
        
        private void RemoveExtraPositions(ICollection<AxialCoordinate> hexPositions)
        {
            for (int i = _lastRenderedPositions.Count - 1; i >= 0; i--)
            {
                var position = _lastRenderedPositions[i];
                
                if (hexPositions.Contains(position))
                    continue;

                _lastRenderedPositions.Remove(position);
                _matricesByPosition.Remove(position);

                _hasChanges = true;
            }
        }

        private void AddNewPositions(float mapScale, ICollection<AxialCoordinate> hexPositions)
        {
            foreach (var position in hexPositions)
            {
                if (_lastRenderedPositions.Contains(position))
                    continue;
                
                _lastRenderedPositions.Add(position);
                _matricesByPosition.Add(position, MatrixBy(position.ToVector3(mapScale)));

                _hasChanges = true;
            }
        }

        private Matrix4x4 MatrixBy(Vector3 worldPosition)
        {
            return Matrix4x4.TRS(worldPosition, Quaternion.Euler(0, 30f, 0f), _hexScale);
        }
    }
}