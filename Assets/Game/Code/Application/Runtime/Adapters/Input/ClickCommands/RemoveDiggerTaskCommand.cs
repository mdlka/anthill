﻿using YellowSquad.Anthill.Core.Tasks;
using YellowSquad.Anthill.Meta;
using YellowSquad.Anthill.UserInput;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Application.Adapters
{
    public class RemoveDiggerTaskCommand : IClickCommand
    {
        private readonly ITaskStorage _diggerTaskStorage;
        private readonly MapCellShop _mapCellShop;

        public RemoveDiggerTaskCommand(ITaskStorage diggerTaskStorage, MapCellShop mapCellShop)
        {
            _diggerTaskStorage = diggerTaskStorage;
            _mapCellShop = mapCellShop;
        }

        public bool TryExecute(ClickInfo clickInfo)
        {
            if (CanExecute(clickInfo.MapPosition) == false)
                return false;
            
            _diggerTaskStorage.CancelTaskGroup(clickInfo.MapPosition);
            _mapCellShop.Sell();
            return true;
        }
        
        private bool CanExecute(AxialCoordinate position)
        {
            return _mapCellShop.CanSellCell &&
                   _diggerTaskStorage.HasTaskGroupIn(position);
        }
    }
}