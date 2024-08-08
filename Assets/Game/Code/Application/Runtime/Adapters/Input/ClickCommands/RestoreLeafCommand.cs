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
        private readonly AddMoneyAnimation _animation;

        public RestoreLeafCommand(IHexMap map, IHexMapView mapView, IWallet wallet, IPriceList mapCellPriceList, AddMoneyAnimation animation)
        {
            _map = map;
            _mapView = mapView;
            _wallet = wallet;
            _mapCellPriceList = mapCellPriceList;
            _animation = animation;
        }

        public bool TryExecute(ClickInfo clickInfo)
        {
            if (CanExecute(clickInfo.MapPosition) == false)
                return false;

            _wallet.Add((int)(_mapCellPriceList.CurrentPrice * RestoreLeafRewardFactor));
            _map.DividedPointOfInterestFrom(clickInfo.MapPosition).Restore();
            _map.Visualize(_mapView, new MapCellChange
            {
                Position = clickInfo.MapPosition,
                AddedParts = _map.DividedPointOfInterestFrom(clickInfo.MapPosition).Parts,
                MapCell = _map.MapCell(clickInfo.MapPosition),
                ChangeType = ChangeType.PointOfInterest
            });
            
            _animation.Play(clickInfo.ScreenPosition, clickInfo.NormalizedZoom);

            return true;
        }
        
        private bool CanExecute(AxialCoordinate position)
        {
            if (_map.HasDividedPointOfInterestIn(position) == false)
                return false;
            
            var targetDividedPointOfInterest = _map.DividedPointOfInterestFrom(position);
            return targetDividedPointOfInterest.HasParts == false && targetDividedPointOfInterest.CanRestore;
        }
    }
}