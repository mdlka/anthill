using System.Collections.Generic;
using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class SolidHexView : MonoBehaviour
    {
        private readonly List<Matrix4x4> _hexesMatrices = new();

        [SerializeField] private Mesh _hexMesh;
        [SerializeField] private Material _hexMaterial;

        private RenderParams _renderParams;
        
        private void Awake()
        {
            _renderParams = new RenderParams(_hexMaterial);
        }

        private void Update()
        {
            if (_hexesMatrices.Count == 0)
                return;

            Graphics.RenderMeshInstanced(_renderParams, _hexMesh, 0, _hexesMatrices);
        }

        public void Render(float mapScale, Vector3 hexScale, IEnumerable<AxialCoordinate> hexPositions)
        {
            foreach (var position in hexPositions)
                _hexesMatrices.Add(Matrix4x4.TRS(position.ToVector3(mapScale), Quaternion.Euler(0f, 30f, 0f), hexScale));
        }

        public void Clear()
        {
            _hexesMatrices.Clear();
        }
    }
}