using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    public class DefaultHex : IHex
    {
        private readonly List<IHexPart> _parts;
        private int _destroyedParts;

        internal DefaultHex(Hardness hardness, IHexMesh mesh) : this(hardness, mesh.Parts()) { }

        private DefaultHex(Hardness hardness, IEnumerable<IHexPart> parts)
        {
            Hardness = hardness;
            _parts = new List<IHexPart>(parts);
            _destroyedParts = _parts.Count(part => part.Destroyed);
        }

        public bool HasParts => _parts.Count - _destroyedParts != 0;
        public bool IsObstacle => HasParts;
        public Hardness Hardness { get; }
        IReadOnlyList<IReadOnlyHexPart> IHex.Parts => _parts;

        public Vector3 ClosestPartLocalPositionFor(AxialCoordinate position)
        {
            if (HasParts == false)
                throw new InvalidOperationException();

            return ClosestPartFor(position.ToVector3()).LocalPosition;
        }

        public void RemoveClosestPartFor(Vector3 localPosition)
        {
            if (HasParts == false)
                throw new InvalidOperationException();

            ClosestPartFor(localPosition).Destroy();
            _destroyedParts += 1;
        }

        private IHexPart ClosestPartFor(Vector3 position)
        {
            if (HasParts == false)
                throw new InvalidOperationException();
            
            IHexPart closestPart = null;
            float closestDistance = int.MaxValue;

            foreach (var part in _parts)
            {
                if (part.Destroyed)
                    continue;
                    
                float distance = Vector3.Distance(part.LocalPosition, position);

                if (distance >= closestDistance) 
                    continue;
                
                closestPart = part;
                closestDistance = distance;
            }
            
            return closestPart;
        }
    }
}