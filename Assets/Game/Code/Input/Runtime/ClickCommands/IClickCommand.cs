using YellowSquad.HexMath;

namespace YellowSquad.Anthill.UserInput
{
    public interface IClickCommand
    {
        bool TryExecute(AxialCoordinate position);
    }
}