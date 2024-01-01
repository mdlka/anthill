using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    internal class AlwaysCompletedTask : ITask
    {
        public AlwaysCompletedTask(AxialCoordinate targetPosition = default)
        {
            TargetPosition = targetPosition;
        }

        public AxialCoordinate TargetPosition { get; }
        public bool Completed => true;
        
        public void Complete() { }
    }
}