using System.Collections.Generic;
using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    public interface IHex
    {
        bool HasParts { get; }
        Hardness Hardness { get; }
        IEnumerable<IReadOnlyHexPart> Parts { get; }

        void DestroyClosestPartFor(Vector3 localPosition);
    }
}