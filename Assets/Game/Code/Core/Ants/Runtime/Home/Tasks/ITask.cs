using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public interface ITask
    {
        AxialCoordinate TargetPosition { get; }
        bool Completed { get; }
        
        void Complete();
    }
}