using System.Collections.Generic;
using UnityEngine;

namespace YellowSquad.Anthill.Core.HexMap
{
    internal interface IHexMesh
    {
        IReadOnlyDictionary<Vector3, Mesh> PartsMeshesByLocalPosition { get; }

        IEnumerable<IHexPart> Parts();
    }
}