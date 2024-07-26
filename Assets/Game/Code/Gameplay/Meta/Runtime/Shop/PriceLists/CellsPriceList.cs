using System;
using YellowSquad.Anthill.Core.HexMap;

namespace YellowSquad.Anthill.Meta
{
    public class CellsPriceList : IPriceList
    {
        private readonly int _addValue;
        private readonly IHexMap _map;

        public CellsPriceList(IHexMap map, int startValue, int addValue)
        {
            _map = map;
            _addValue = addValue;
            CurrentPrice = startValue;
        }

        public int CurrentPriceNumber { get; private set; } = 1;
        public int CurrentPrice { get; private set; }
        public bool HasNext => CurrentPriceNumber < _map.TotalCells;
        
        public void Next()
        {
            CurrentPrice += CalculateAddedValue();
            CurrentPriceNumber += 1;
        }
        
        private int CalculateAddedValue()
        {
            float addedValue = _addValue - CurrentPriceNumber * _addValue / _map.TotalCells;
            return (int)Math.Round(addedValue);
        }
    }
}