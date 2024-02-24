using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Input
{
    public interface IClickCommand
    {
        bool CanExecute(AxialCoordinate position);
        void Execute(AxialCoordinate position);
    }
}