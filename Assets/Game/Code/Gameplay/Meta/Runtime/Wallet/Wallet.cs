using System;

namespace YellowSquad.Anthill.Meta
{
    public class Wallet : IWallet
    {
        public Wallet(int startValue = 0)
        {
            if (startValue < 0)
                throw new ArgumentOutOfRangeException(nameof(startValue));

            CurrentValue = startValue;
        }
        
        public int CurrentValue { get; private set; }
        
        public void Add(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            CurrentValue += value;
        }

        public void Spend(int value)
        {
            if (CanSpend(value) == false)
                throw new InvalidOperationException();

            CurrentValue -= value;
        }

        public bool CanSpend(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            return CurrentValue - value >= 0;
        }
    }
}