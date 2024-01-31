using System;
using System.Collections.Generic;
using UnityEngine;
using YellowSquad.AssetPath;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal abstract class BaseDividedObjectMesh : ScriptableObject, IDividedObjectMesh
    {
        [SerializeField] private ResourcesReference<GameObject> _modelObject;

        private Pair[] _cachedMesh;
        private Dictionary<Vector3, Mesh> _cachedPartsMeshesByLocalPosition;

        public IReadOnlyDictionary<Vector3, Mesh> PartsMeshesByLocalPosition
        {
            get
            {
                if (_cachedPartsMeshesByLocalPosition != null)
                    return _cachedPartsMeshesByLocalPosition;
                
                var meshes = LoadMeshes();
                _cachedPartsMeshesByLocalPosition = new Dictionary<Vector3, Mesh>();

                foreach (var pair in meshes)
                    _cachedPartsMeshesByLocalPosition.Add(pair.CenterPosition, pair.Mesh);

                return _cachedPartsMeshesByLocalPosition;
            } 
        }

        public IEnumerable<IPart> Parts()
        {
            var meshes = LoadMeshes();
            var parts = new IPart[meshes.Length];

            for (int i = 0; i < meshes.Length; i++)
                parts[i] = new Part(meshes[i].CenterPosition);

            return parts;
        }

        private Pair[] LoadMeshes()
        {
            if (_cachedMesh != null)
                return _cachedMesh;

            if (_modelObject.IsNull)
                return _cachedMesh = Array.Empty<Pair>();
            
            var hexModel = _modelObject.Load();

            if (hexModel == null)
                throw new InvalidOperationException("Object doesn't exist");

            var hexParts = hexModel.GetComponentsInChildren<MeshFilter>();
            _cachedMesh = new Pair[hexParts.Length];

            for (int i = 0; i < hexParts.Length; i++)
                _cachedMesh[i] = new Pair(hexParts[i].sharedMesh, hexParts[i].transform.position);

            return _cachedMesh;
        }

        private struct Pair
        {
            public Pair(Mesh mesh, Vector3 centerPosition)
            {
                Mesh = mesh;
                CenterPosition = centerPosition;
            }

            public Mesh Mesh { get; }
            public Vector3 CenterPosition { get; }
        }
    }
}