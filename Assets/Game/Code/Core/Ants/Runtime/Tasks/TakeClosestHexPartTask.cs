using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class TakeClosestHexPartTask : ITask
    {
        private readonly float _mapScale;
        private readonly IHex _targetHex;

        public TakeClosestHexPartTask(float mapScale, AxialCoordinate targetCellPosition, IHex targetHex)
        {
            TargetCellPosition = targetCellPosition;
            _mapScale = mapScale;
            _targetHex = targetHex;
        }
        
        public AxialCoordinate TargetCellPosition { get; }
        public bool Completed { get; private set; }
        
        public void Complete(FracAxialCoordinate currentPosition)
        {
            _targetHex.DestroyClosestPartFor((currentPosition - TargetCellPosition).ToVector3(_mapScale));
            Completed = true;
        }
        
        public bool Equals(ITask other)
        {
            return false;
        }
    }
}