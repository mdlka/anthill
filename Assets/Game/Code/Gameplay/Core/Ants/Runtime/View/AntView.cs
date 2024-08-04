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

        private readonly List<Matrix4x4> _partsMatrices = new();

        [SerializeField] private Mesh _mesh;
        [SerializeField] private Material _material;
        [SerializeField, Min(0f)] private float _scale;
        [SerializeField] private float _rotationX;
        [SerializeField] private bool _invertRotationY;
        [Header("Part")] 
        [SerializeField] private float _partScale;
        [SerializeField] private Vector3 _partOffset;
        [SerializeField] private Material _partMaterial;
        [SerializeField] private Mesh _partMesh;

        private RenderParams _renderParams;
        private RenderParams _partRenderParams;
        private float _mapScale;

        public void Initialize(float mapScale)
        {
            _mapScale = mapScale;
            _renderParams = new RenderParams(_material);
            _partRenderParams = new RenderParams(_partMaterial);
        }

        public void Add(IAnt ant)
        {
            if (_ants.Contains(ant))
                throw new InvalidOperationException();
            
            _ants.Add(ant);
            _lastPositions.Add(ant.CurrentPosition);
            _lastRotationY.Add(0f);
            _matrices.Add(WorldMatrixFor(ant.CurrentPosition.ToVector3(_mapScale), 0f, _scale));
            _partsMatrices.Add(Matrix4x4.zero);
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
                
                _matrices[i] = WorldMatrixFor(currentPosition, _lastRotationY[i], _scale);
                _lastPositions[i] = _ants[i].CurrentPosition;

                _partsMatrices[i] = _ants[i].HasPart
                    ? WorldMatrixFor(currentPosition + _partOffset, _lastRotationY[i], _partScale)
                    : WorldMatrixFor(Vector3.zero, 0, 0);
            }

            Graphics.RenderMeshInstanced(_renderParams, _mesh, 0, _matrices);
            Graphics.RenderMeshInstanced(_partRenderParams, _partMesh, 0, _partsMatrices);
        }

        private Matrix4x4 WorldMatrixFor(Vector3 currentPosition, float rotationY, float scale)
        {
            if (_invertRotationY)
                rotationY += 180;
            
            return Matrix4x4.TRS(currentPosition, Quaternion.Euler(_rotationX, rotationY, 0f), Vector3.one * scale);
        }
    }
}