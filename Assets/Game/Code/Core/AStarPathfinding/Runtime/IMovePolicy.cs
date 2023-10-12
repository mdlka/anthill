using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.AStarPathfinding
{
    public interface IMovePolicy
    {
        bool CanMove(AxialCoordinate axialCoordinate);
    }
}
