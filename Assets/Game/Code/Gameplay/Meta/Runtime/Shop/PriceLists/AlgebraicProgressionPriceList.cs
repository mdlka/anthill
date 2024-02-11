using System;

namespace YellowSquad.Anthill.Meta
{
    public class AlgebraicProgressionPriceList : IPriceList
    {
        private readonly int _addValue;

        public AlgebraicProgressionPriceList(int startPrice, int addValue)
        {
            if (startPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(startPrice));
            
            if (addValue <= 0)
                throw new ArgumentOutOfRangeException(nameof(addValue));

            CurrentPrice = startPrice;
            _addValue = addValue;
        }

        public int CurrentPriceNumber { get; private set; } = 1;
        public int CurrentPrice { get; private set; }
        public bool HasNext => true;

        public void Next()
        {
            CurrentPriceNumber += 1;
            CurrentPrice += _addValue;
        }
    }
}