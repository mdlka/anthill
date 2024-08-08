using YellowSquad.Anthill.Core.Tasks;
using YellowSquad.Anthill.UserInput;
using YellowSquad.Anthill.Meta;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Application.Adapters
{
    public class AddDiggerTaskCommand : IClickCommand
    {
        private readonly ITaskStorage _diggerTaskStorage;
        private readonly ITaskGroupFactory _collectHexTaskGroupFactory;
        private readonly MapCellShop _mapCellShop;

        public AddDiggerTaskCommand(ITaskStorage diggerTaskStorage, ITaskGroupFactory collectHexTaskGroupFactory, MapCellShop mapCellShop)
        {
            _diggerTaskStorage = diggerTaskStorage;
            _collectHexTaskGroupFactory = collectHexTaskGroupFactory;
            _mapCellShop = mapCellShop;
        }

        public bool TryExecute(ClickInfo clickInfo)
        {
            if (CanExecute(clickInfo.MapPosition) == false)
                return false;
            
            _diggerTaskStorage.AddTaskGroup(_collectHexTaskGroupFactory.Create(clickInfo.MapPosition));
            _mapCellShop.Buy();
            return true;
        }
        
        private bool CanExecute(AxialCoordinate position)
        {
            return _mapCellShop.CanBuyCell &&
                   _diggerTaskStorage.HasTaskGroupIn(position) == false &&
                   _collectHexTaskGroupFactory.CanCreate(position);
        }
    }
}