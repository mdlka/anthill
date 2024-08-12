using YellowSquad.Anthill.UserInput;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Tutorial
{
    internal class TutorialClickCommand : IClickCommand
    {
        private readonly AxialCoordinate _targetPosition;

        public TutorialClickCommand(AxialCoordinate targetPosition)
        {
            _targetPosition = targetPosition;
        }
        
        public bool TargetHexClicked { get; private set; }
        
        public bool TryExecute(ClickInfo clickInfo)
        {
            if (TargetHexClicked)
                return false;

            if (clickInfo.MapPosition == _targetPosition)
                TargetHexClicked = true;

            return true;
        }
    }
}