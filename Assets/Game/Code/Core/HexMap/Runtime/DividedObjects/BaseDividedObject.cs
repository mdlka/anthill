using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YellowSquad.Anthill.Core.HexMap
{
    public abstract class BaseDividedObject : IDividedObject
    {
        private readonly List<IPart> _parts;
        private int _destroyedParts;

        internal BaseDividedObject(Hardness hardness, IDividedObjectMesh mesh) : this(hardness, mesh.Parts()) { }

        private BaseDividedObject(Hardness hardness, IEnumerable<IPart> parts)
        {
            Hardness = hardness;
            _parts = new List<IPart>(parts);
            _destroyedParts = _parts.Count(part => part.Destroyed);
        }

        public bool HasParts => _parts.Count - _destroyedParts != 0;
        public Hardness Hardness { get; }
        public IEnumerable<IReadOnlyPart> Parts => _parts;

        public void DestroyClosestPartFor(Vector3 localPosition)
        {
            ClosestPartFor(localPosition).Destroy();
            _destroyedParts += 1;
        }

        private IPart ClosestPartFor(Vector3 localPosition)
        {
            if (HasParts == false)
                throw new InvalidOperationException();
            
            IPart closestPart = null;
            float closestDistance = int.MaxValue;

            foreach (var part in _parts)
            {
                if (part.Destroyed)
                    continue;
                    
                float distance = Vector3.Distance(part.LocalPosition, localPosition);

                if (distance >= closestDistance) 
                    continue;
                
                closestPart = part;
                closestDistance = distance;
            }
            
            return closestPart;
        }
    }
}