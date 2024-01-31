using System;
using System.Collections.Generic;
using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class AntView : MonoBehaviour
    {
        private readonly List<IAnt> _ants = new();
        private readonly List<FracAxialCoordinate> _lastPositions = new();
        private readonly List<Matrix4x4> _matrices = new();
        private readonly List<float> _lastRotationY = new();

        [SerializeField] private Mesh _mesh;
        [SerializeField] private Material _material;
        [SerializeField, Min(0f)] private float _scale;

        private RenderParams _renderParams;
        private float _mapScale;

        public void Initialize(float mapScale)
        {
            _mapScale = mapScale;
            _renderParams = new RenderParams(_material);
        }

        public void Add(IAnt ant)
        {
            if (_ants.Contains(ant))
                throw new InvalidOperationException();
            
            _ants.Add(ant);
            _lastPositions.Add(ant.CurrentPosition);
            _lastRotationY.Add(0f);
            _matrices.Add(WorldMatrixFor(ant.CurrentPosition.ToVector3(_mapScale), 0f));
        }
        
        public void UpdateRender()
        {
            if (_ants.Count == 0)
                return;

            for (int i = 0; i < _ants.Count; i++)
            {
                if (_ants[i].Moving == false && _lastPositions[i] == _ants[i].CurrentPosition) 
                    continue;
                
                Vector3 currentPosition = _ants[i].CurrentPosition.ToVector3(_mapScale);
                Vector3 lastPosition = _lastPositions[i].ToVector3(_mapScale);
                
                _lastRotationY[i] = Vector3.Distance(currentPosition, lastPosition) > 0.01f
                    ? Quaternion.LookRotation((lastPosition - currentPosition).normalized).eulerAngles.y
                    : _lastRotationY[i];
                
                _matrices[i] = WorldMatrixFor(currentPosition, _lastRotationY[i]);
                _lastPositions[i] = _ants[i].CurrentPosition;
            }

            Graphics.RenderMeshInstanced(_renderParams, _mesh, 0, _matrices);
        }

        private Matrix4x4 WorldMatrixFor(Vector3 currentPosition, float rotationY)
        {
            return Matrix4x4.TRS(currentPosition, Quaternion.Euler(-90f, rotationY, 0f), Vector3.one * _scale);
        }
    }
}