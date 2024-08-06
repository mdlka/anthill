using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class GroundMapView : MonoBehaviour
    {
        private readonly List<List<Matrix4x4>> _matrices = new();
        private readonly List<RenderParams> _renderParams = new();

        [SerializeField] private Mesh _hexMesh;
        [SerializeField] private Material[] _hexMaterials;
        [SerializeField] private float _factor;

        private float _mapScale;
        private Vector3 _hexScale;
        private IEnumerable<AxialCoordinate> _positions;

        public bool Rendered { get; private set; }
        
        private void Update()
        {
            if (_matrices.Count == 0)
                return;

            // foreach (var matrices in _matrices)
            //     matrices.Clear();
            //
            // Vector3 offsetY = Vector3.up * _hexMesh.bounds.size.y;
            // Vector3 groundHexScale = new Vector3(_hexScale.x, 1f, _hexScale.z);
            //
            // foreach (var position in _positions)
            // {
            //     int index = Mathf.RoundToInt(Mathf.Clamp01(Mathf.PerlinNoise(position.Q / _factor, position.R / _factor)) * _hexMaterials.Length);
            //     _matrices[index].Add(Matrix4x4.TRS(position.ToVector3(_mapScale) - offsetY, Quaternion.Euler(0f, 30f, 0f), groundHexScale));
            // }

            for (int i = 0; i < _matrices.Count; i++)
                if (_matrices[i].Count > 0)
                    Graphics.RenderMeshInstanced(_renderParams[i], _hexMesh, 0, _matrices[i]);
        }

        public void Render(float mapScale, Vector3 hexScale, IEnumerable<AxialCoordinate> hexPositions)
        {
            if (Rendered)
                throw new InvalidOperationException("Already initialized");

            _mapScale = mapScale;
            _hexScale = hexScale;
            _positions = hexPositions.ToList();
            
            foreach (var material in _hexMaterials)
            {
                _renderParams.Add(new RenderParams(material));
                _matrices.Add(new List<Matrix4x4>());
            }
            
            Vector3 offsetY = Vector3.up * _hexMesh.bounds.size.y;
            Vector3 groundHexScale = new Vector3(hexScale.x, 1f, hexScale.z);
            
            foreach (var position in _positions)
            {
                float value = Mathf.Clamp01(Mathf.PerlinNoise(position.Q * _factor, position.R * _factor));
                int index = Mathf.Clamp((int)(value * _hexMaterials.Length), 0, _hexMaterials.Length - 1);
                
                _matrices[index].Add(Matrix4x4.TRS(position.ToVector3(mapScale) - offsetY, Quaternion.Euler(0f, 30f, 0f), groundHexScale));
            }

            Rendered = true;
        }
    }
}