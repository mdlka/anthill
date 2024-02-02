using System;

namespace YellowSquad.Anthill.Meta
{
    internal interface IPriceList
    {
        int CurrentPriceNumber { get; }
        int CurrentPrice{ get; }
        
        void Next();
    }
    
    internal class AlgebraicProgressionPriceList : IPriceList
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
        
        public void Next()
        {
            CurrentPriceNumber += 1;
            CurrentPrice += _addValue;
        }
    }
}