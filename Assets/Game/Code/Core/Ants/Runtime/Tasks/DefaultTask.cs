using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class DefaultTask : ITask
    {
        public DefaultTask(AxialCoordinate targetPosition)
        {
            TargetPosition = targetPosition;
        }
        
        public AxialCoordinate TargetPosition { get; }
        public bool Completed { get; private set; }
        
        public void Complete()
        {
            Completed = true;
        }
    }
}