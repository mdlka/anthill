using System;
using UnityEngine;

namespace YellowSquad.Anthill.Meta.Shop
{
    internal class LinearPriceList : IPriceList
    {
        private readonly int _pricesCount;
        private readonly int _startPrice;
        private readonly int _maxPrice;

        public LinearPriceList(int pricesCount, int startPrice, int maxPrice)
        {
            if (pricesCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(pricesCount));
            
            if (startPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(startPrice));
            
            if (maxPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(maxPrice));
            
            _pricesCount = pricesCount;
            _startPrice = startPrice;
            _maxPrice = maxPrice;

            CurrentPrice = _startPrice;
        }

        public bool HasNext => CurrentPriceNumber < _pricesCount;
        public int CurrentPriceNumber { get; private set; } = 1;
        public int CurrentPrice { get; private set; }

        public void Next()
        {
            if (HasNext == false)
                throw new InvalidOperationException();
            
            CurrentPriceNumber += 1;
            CurrentPrice = (int)Mathf.Lerp(_startPrice, _maxPrice, (float)(CurrentPriceNumber - 1) / (_pricesCount - 1));
        }
    }
}