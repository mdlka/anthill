using System.Collections.Generic;
using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class HexView : MonoBehaviour
    {
        private readonly Dictionary<AxialCoordinate, Matrix4x4> _matricesByPosition = new();
        private readonly List<AxialCoordinate> _lastRenderedPositions = new();
        private readonly List<Matrix4x4> _matrices = new();

        [SerializeField] private Material _hexMaterial;
        [SerializeField] private string _targetHexModelPath;
        
        private bool _hasChanges;
        private RenderParams _renderParams;
        private Mesh[] _hexMeshes;
        
        private void Awake()
        {
            _renderParams = new RenderParams(_hexMaterial);
            _hexMeshes = Resources.LoadAll<Mesh>(_targetHexModelPath);
        }
        
        private void Update()
        {
            if (_matrices.Count == 0)
                return;
            
            //Graphics.RenderMeshInstanced(_renderParams, _hexMesh, 0, _matrices);
        }

        public void Render(float mapScale, Vector3 hexPosition, IReadOnlyCollection<Vector3> partsLocalPositions)
        {
        }
    }
}