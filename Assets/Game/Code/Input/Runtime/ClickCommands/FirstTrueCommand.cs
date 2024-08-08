using System.Linq;

namespace YellowSquad.Anthill.UserInput
{
    public class FirstTrueCommand : IClickCommand
    {
        private readonly IClickCommand[] _commands;

        public FirstTrueCommand(params IClickCommand[] commands)
        {
            _commands = commands;
        }

        public bool TryExecute(ClickInfo clickInfo)
        {
            return _commands.Any(command => command.TryExecute(clickInfo));
        }
    }
}