using YellowSquad.Anthill.Core.CameraControl;
using YellowSquad.GameLoop;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Input
{
    public class InputRoot : IGameLoop
    {
        private readonly IInput _input;
        private readonly ICamera _camera;
        private readonly IClickCommand[] _clickCommands;

        public InputRoot(IInput input, ICamera camera, params IClickCommand[] commands)
        {
            _input = input;
            _camera = camera;
            _clickCommands = commands;
        }

        public void Update(float deltaTime)
        {
            if (_input.Zoomed(out float delta))
                _camera.Zoom(-delta * deltaTime, () => _input.CursorPosition);
            
            if (_input.ClickedOnOpenMapPosition(out AxialCoordinate clickPosition) == false)
                return;

            foreach (var clickCommand in _clickCommands)
                if (clickCommand.CanExecute(clickPosition))
                    clickCommand.Execute(clickPosition);
        }
    }
}