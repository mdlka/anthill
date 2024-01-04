using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class TakePartHexTask : ITask
    {
        private readonly IHex _targetHex;
        private readonly IReadOnlyHexPart _targetPart;

        public TakePartHexTask(AxialCoordinate targetCellPosition, IHex targetHex, IReadOnlyHexPart targetPart)
        {
            TargetCellPosition = targetCellPosition;
            _targetHex = targetHex;
            _targetPart = targetPart;
        }
        
        public AxialCoordinate TargetCellPosition { get; }
        public bool Completed => _targetPart.Destroyed;
        
        public void Complete()
        {
            _targetHex.DestroyClosestPartFor(_targetPart.LocalPosition);
        }

        public bool Equals(ITask other)
        {
            return other is TakePartHexTask otherTakePartHexTask
                   && TargetCellPosition == other.TargetCellPosition
                   && _targetPart.LocalPosition == otherTakePartHexTask._targetPart.LocalPosition;
        }
    }
}