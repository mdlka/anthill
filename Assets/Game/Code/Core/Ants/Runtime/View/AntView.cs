using System;
using System.Collections.Generic;
using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class AntView : MonoBehaviour
    {
        private readonly List<IAnt> _ants = new();
        private readonly List<Matrix4x4> _matrices = new();
        
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
            _matrices.Add(WorldMatrixFor(ant));
        }
        
        public void UpdateRender()
        {
            if (_ants.Count == 0)
                return;
            
            for (int i = 0; i < _ants.Count; i++)
                if (_ants[i].Moving)
                    _matrices[i] = WorldMatrixFor(_ants[i]);

            Graphics.RenderMeshInstanced(_renderParams, _mesh, 0, _matrices);
        }

        private Matrix4x4 WorldMatrixFor(IAnt ant)
        {
            return Matrix4x4.TRS(ant.CurrentPosition.ToVector3(_mapScale), Quaternion.Euler(-90f, 0f, 0f), Vector3.one * _scale);
        }
    }
}