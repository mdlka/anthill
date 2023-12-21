using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    internal interface IAnt : IGameLoop
    {
        AxialCoordinate CurrentPosition { get; }
    }
}