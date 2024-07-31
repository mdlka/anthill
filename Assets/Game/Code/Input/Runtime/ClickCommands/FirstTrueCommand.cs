using System.Linq;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.UserInput
{
    public class FirstTrueCommand : IClickCommand
    {
        private readonly IClickCommand[] _commands;

        public FirstTrueCommand(params IClickCommand[] commands)
        {
            _commands = commands;
        }

        public bool TryExecute(AxialCoordinate position)
        {
            return _commands.Any(command => command.TryExecute(position));
        }
    }
}