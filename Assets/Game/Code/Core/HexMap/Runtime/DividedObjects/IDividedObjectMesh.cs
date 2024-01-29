using System.Collections.Generic;
using UnityEngine;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal interface IDividedObjectMesh
    {
        IReadOnlyDictionary<Vector3, Mesh> PartsMeshesByLocalPosition { get; }

        IEnumerable<IPart> Parts();
    }
}