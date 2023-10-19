using System;
using System.Collections.Generic;
using UnityEngine;

namespace YellowSquad.Anthill.Core.HexMap
{
    [CreateAssetMenu(menuName = "Anthill/Create HexMesh", fileName = "HexMesh", order = 56)]
    internal class HexMesh : ScriptableObject, IHexMesh
    {
        [SerializeField] private string _objectPath;

        private List<IHexPart> _cachedParts;

        public IEnumerable<IHexPart> Parts
        {
            get
            {
                if (_cachedParts != null)
                    return _cachedParts;

                var meshes = Resources.LoadAll<Mesh>(_objectPath);

                if (meshes == null || meshes.Length == 0)
                    throw new InvalidOperationException("Object doesn't exist");
                
                _cachedParts = new List<IHexPart>();

                foreach (var mesh in meshes)
                    _cachedParts.Add(new HexPart(mesh.bounds.center));

                return _cachedParts;
            }
        }
    }
}