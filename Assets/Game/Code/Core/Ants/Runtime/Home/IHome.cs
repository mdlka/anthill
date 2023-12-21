using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    internal interface IHome : IReadOnlyTaskStorage
    {
        AxialCoordinate Position { get; }
    }
}