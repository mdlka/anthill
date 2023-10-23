using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    public abstract class BaseHex : IHex
    {
        private readonly List<IHexPart> _parts;
        private int _renderedParts;

        internal BaseHex(IHexMesh mesh) : this(mesh.CreateParts()) { }

        internal BaseHex(IEnumerable<IHexPart> parts)
        {
            _parts = new List<IHexPart>(parts);
            _renderedParts = _parts.Count(part => part.NeedRender);
        }

        public bool HasParts => _renderedParts != 0;

        IReadOnlyList<IReadOnlyHexPart> IHex.Parts => _parts;

        public abstract bool IsObstacle { get; }

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

            ClosestPartFor(localPosition).Disable();
            _renderedParts -= 1;
        }

        private IHexPart ClosestPartFor(Vector3 position)
        {
            if (HasParts == false)
                throw new InvalidOperationException();
            
            IHexPart closestPart = null;
            float closestDistance = int.MaxValue;

            foreach (var part in _parts)
            {
                if (part.NeedRender == false)
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