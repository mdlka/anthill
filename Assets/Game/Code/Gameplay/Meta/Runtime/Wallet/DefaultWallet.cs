using System;

namespace YellowSquad.Anthill.Meta
{
    public class DefaultWallet : IWallet
    {
        private readonly IWalletView _view;

        public DefaultWallet(IWalletView view, int startValue = 0)
        {
            if (startValue < 0)
                throw new ArgumentOutOfRangeException(nameof(startValue));
            
            _view = view;
            CurrentValue = startValue;
        }
        
        public int CurrentValue { get; private set; }
        
        public void Add(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            CurrentValue += value;
            _view.Render(CurrentValue);
        }

        public void Spend(int value)
        {
            if (CanSpend(value) == false)
                throw new InvalidOperationException();

            CurrentValue -= value;
            _view.Render(CurrentValue);
        }

        public bool CanSpend(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            return CurrentValue - value >= 0;
        }
    }
}