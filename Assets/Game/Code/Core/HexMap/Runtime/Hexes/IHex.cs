using System.Collections.Generic;
using UnityEngine;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.HexMap
{
    public interface IHex
    {
        bool IsObstacle { get; }
        bool HasParts { get; }
        internal IReadOnlyList<IReadOnlyHexPart> Parts { get; }

        Vector3 ClosestPartLocalPositionFor(AxialCoordinate position);
        void RemoveClosestPartFor(Vector3 localPosition);
    }
}