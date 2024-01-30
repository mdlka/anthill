using System.Collections.Generic;
using UnityEngine;

namespace YellowSquad.Anthill.Core.HexMap
{
    public interface IDividedObject
    {
        bool HasParts { get; }
        bool CanRestore { get; }
        Hardness Hardness { get; }
        IEnumerable<IReadOnlyPart> Parts { get; }

        void DestroyClosestPartFor(Vector3 localPosition);
        void Restore();
    }
}