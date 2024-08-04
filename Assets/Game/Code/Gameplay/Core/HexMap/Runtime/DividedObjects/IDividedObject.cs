using System.Collections.Generic;
using UnityEngine;

namespace YellowSquad.Anthill.Core.HexMap
{
    public interface IDividedObject
    {
        bool HasParts { get; }
        bool CanRestore { get; }
        Hardness Hardness { get; }
        IEnumerable<IPart> Parts { get; }

        void DestroyClosestPartFor(Vector3 localPosition);
        void Restore();
    }
}