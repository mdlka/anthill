using YellowSquad.GameLoop;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Input
{
    public class InputRoot : IGameLoop
    {
        private readonly IInput _input;
        private readonly IClickCommand[] _clickCommands;

        public InputRoot(IInput input, params IClickCommand[] commands)
        {
            _input = input;
            _clickCommands = commands;
        }

        public void Update(float deltaTime)
        {
            if (_input.ClickedOnOpenMapPosition(out AxialCoordinate clickPosition) == false)
                return;

            foreach (var clickCommand in _clickCommands)
                if (clickCommand.CanExecute(clickPosition))
                    clickCommand.Execute(clickPosition);
        }
    }
}