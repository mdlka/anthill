using System.Collections.Generic;

namespace YellowSquad.Anthill.Core.HexMap
{
    public interface IHexMesh
    {
        IEnumerable<IHexPart> Parts { get; }
    }
}