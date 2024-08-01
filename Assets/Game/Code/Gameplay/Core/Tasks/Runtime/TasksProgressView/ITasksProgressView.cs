using System.Collections.Generic;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public interface ITasksProgressView
    {
        void Render(Dictionary<AxialCoordinate, ITaskGroup> activeTaskGroups);
    }
}