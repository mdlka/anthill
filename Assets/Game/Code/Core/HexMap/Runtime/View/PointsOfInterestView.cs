using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal class PointsOfInterestView : MonoBehaviour
    {
        private readonly Dictionary<PointOfInterest, List<Matrix4x4>> _pointsMatrices = new();

        [SerializeField] private Material _material;
        [SerializeField] private List<PointSetting> _meshes;

        private RenderParams _renderParams;
        
        public bool Rendered { get; private set; }
        
        private void Awake()
        {
            _renderParams = new RenderParams(_material);
        }

        private void Update()
        {
            if (_pointsMatrices.Count == 0)
                return;

            foreach (var pair in _pointsMatrices)
            {
                if (pair.Value.Count == 0)
                    return;
                
                Graphics.RenderMeshInstanced(_renderParams, SettingBy(pair.Key).Mesh, 0, pair.Value);
            }
        }

        public void Render(float mapScale, IDictionary<AxialCoordinate, PointOfInterest> pointsPositions)
        {
            if (Rendered)
                throw new InvalidOperationException("Already initialized");

            foreach (var pair in pointsPositions)
            {
                if (pair.Value == PointOfInterest.Empty)
                    continue;

                if (_pointsMatrices.ContainsKey(pair.Value) == false)
                    _pointsMatrices.Add(pair.Value, new List<Matrix4x4>());

                var setting = SettingBy(pair.Value);
                _pointsMatrices[pair.Value].Add(Matrix4x4.TRS(pair.Key.ToVector3(mapScale), 
                    Quaternion.Euler(setting.Rotation), Vector3.one * setting.SizeFactor));
            }

            Rendered = true;
        }

        private PointSetting SettingBy(PointOfInterest pointOfInterest)
        {
            return _meshes.First(mesh => mesh.PointOfInterest == pointOfInterest);
        }
        
        [Serializable]
        private class PointSetting
        {
            [field: SerializeField] public PointOfInterest PointOfInterest { get; private set; }
            [field: SerializeField] public Mesh Mesh { get; private set; }
            [field: SerializeField, Min(0f)] public float SizeFactor { get; private set; }
            [field: SerializeField] public Vector3 Rotation { get; private set; }
        }
    }
}