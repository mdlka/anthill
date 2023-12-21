using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public interface IAnt : IGameLoop
    {
        AxialCoordinate CurrentPosition { get; }
    }
}