using System.Collections.Generic;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.AStarPathfinding
{
    public interface IPath
    {
        bool Calculate(AxialCoordinate start, AxialCoordinate target, out IReadOnlyList<AxialCoordinate> path);
    }
}
