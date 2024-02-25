using System.Collections.Generic;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public interface ITasksProgressView
    {
        void Render(HashSet<AxialCoordinate> activeTaskGroupsPositions);
    }
}