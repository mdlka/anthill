using System;
using System.Collections.Generic;
using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    public abstract class BaseHex : IHex
    {
        private readonly List<IHexPart> _parts;

        protected BaseHex(IHexMesh mesh) : this(mesh.Parts) { }

        internal BaseHex(IEnumerable<IHexPart> parts)
        {
            _parts = new List<IHexPart>(parts);
        }

        public bool HasParts => _parts.Count != 0;

        IReadOnlyList<IReadOnlyHexPart> IHex.Parts => _parts;

        public abstract bool IsObstacle { get; }

        public Vector3 ClosestPartLocalPositionFor(AxialCoordinate position)
        {
            if (HasParts == false)
                throw new InvalidOperationException();

            return ClosestPartFor(position.ToVector3()).Position;
        }

        public void RemoveClosestPartFor(Vector3 localPosition)
        {
            if (HasParts == false)
                throw new InvalidOperationException();

            ClosestPartFor(localPosition).Disable();
        }

        private IHexPart ClosestPartFor(Vector3 position)
        {
            IHexPart closestPart = null;
            float closestDistance = int.MaxValue;

            foreach (var part in _parts)
            {
                float distance = Vector3.Distance(part.Position, position);

                if (distance >= closestDistance) 
                    continue;
                
                closestPart = part;
                closestDistance = distance;
            }
            
            return closestPart;
        }
    }
}