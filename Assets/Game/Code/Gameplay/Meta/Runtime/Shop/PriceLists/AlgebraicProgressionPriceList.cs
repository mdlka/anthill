using System;

namespace YellowSquad.Anthill.Meta
{
    public class AlgebraicProgressionPriceList : IPriceList
    {
        private readonly int _startPrice;
        private readonly int _addValue;

        public AlgebraicProgressionPriceList(int startPrice, int addValue, int startPriceNumber = 0)
        {
            if (startPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(startPrice));
            
            if (addValue <= 0)
                throw new ArgumentOutOfRangeException(nameof(addValue));

            if (startPriceNumber < 0)
                throw new ArgumentOutOfRangeException(nameof(startPriceNumber));

            _startPrice = startPrice;
            _addValue = addValue;

            CurrentPriceNumber = startPriceNumber;
        }

        public int CurrentPriceNumber { get; private set; }
        public int CurrentPrice => _startPrice + _addValue * CurrentPriceNumber;
        public bool HasNext => true;
        public bool HasPrevious => CurrentPriceNumber >= 1;

        public void Next()
        {
            if (HasNext == false)
                throw new InvalidOperationException();
            
            CurrentPriceNumber += 1;
        }

        public void Previous()
        {
            if (HasPrevious == false)
                throw new InvalidOperationException();
            
            CurrentPriceNumber -= 1;
        }
    }
}