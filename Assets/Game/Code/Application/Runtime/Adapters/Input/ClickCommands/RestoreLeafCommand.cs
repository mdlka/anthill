using System;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.Anthill.UserInput;
using YellowSquad.Anthill.Meta;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Application.Adapters
{
    public class RestoreLeafCommand : IClickCommand
    {
        private const float RestoreLeafRewardFactor = 0.4f;
        
        private readonly IHexMap _map;
        private readonly IHexMapView _mapView;
        private readonly IWallet _wallet;
        private readonly IPriceList _mapCellPriceList;

        public RestoreLeafCommand(IHexMap map, IHexMapView mapView, IWallet wallet, IPriceList mapCellPriceList)
        {
            _map = map;
            _mapView = mapView;
            _wallet = wallet;
            _mapCellPriceList = mapCellPriceList;
        }

        public bool CanExecute(AxialCoordinate position)
        {
            if (_map.HasDividedPointOfInterestIn(position) == false)
                return false;
            
            var targetDividedPointOfInterest = _map.DividedPointOfInterestFrom(position);
            return targetDividedPointOfInterest.HasParts == false && targetDividedPointOfInterest.CanRestore;
        }

        public void Execute(AxialCoordinate position)
        {
            if (CanExecute(position) == false)
                throw new InvalidOperationException();

            _wallet.Add((int)(_mapCellPriceList.CurrentPrice * RestoreLeafRewardFactor));
            _map.DividedPointOfInterestFrom(position).Restore();
            _map.Visualize(_mapView);
        }
    }
}