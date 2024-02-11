using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Input
{
    public interface IInput
    {
        bool Clicked(out AxialCoordinate position);
    }
}
