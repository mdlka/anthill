using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public interface ITaskStore
    {
        void Sell(ITask task, AxialCoordinate sellPosition);
    }
}