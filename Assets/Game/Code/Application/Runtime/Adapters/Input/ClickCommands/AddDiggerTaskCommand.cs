using System;
using YellowSquad.Anthill.Core.Tasks;
using YellowSquad.Anthill.Input;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Application.Adapters
{
    public class AddDiggerTaskCommand : IClickCommand
    {
        private readonly ITaskStorage _diggerTaskStorage;
        private readonly ITaskGroupFactory _collectHexTaskGroupFactory;

        public AddDiggerTaskCommand(ITaskStorage diggerTaskStorage, ITaskGroupFactory collectHexTaskGroupFactory)
        {
            _diggerTaskStorage = diggerTaskStorage;
            _collectHexTaskGroupFactory = collectHexTaskGroupFactory;
        }

        public bool CanExecute(AxialCoordinate position)
        {
            return _diggerTaskStorage.HasTaskGroupIn(position) == false &&
                   _collectHexTaskGroupFactory.CanCreate(position);
        }

        public void Execute(AxialCoordinate position)
        {
            if (CanExecute(position) == false)
                throw new InvalidOperationException();
            
            _diggerTaskStorage.AddTaskGroup(_collectHexTaskGroupFactory.Create(position));
        }
    }
}