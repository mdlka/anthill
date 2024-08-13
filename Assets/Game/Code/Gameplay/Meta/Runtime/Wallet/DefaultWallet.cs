using System;
using YellowSquad.GamePlatformSdk;

namespace YellowSquad.Anthill.Meta
{
    public class DefaultWallet : IWallet
    {
        private readonly IWalletView _view;
        private readonly ISave _save;

        public DefaultWallet(IWalletView view, ISave save, int startValue = 0)
        {
            if (startValue < 0)
                throw new ArgumentOutOfRangeException(nameof(startValue));
            
            _view = view;
            _save = save;

            CurrentValue = _save.GetInt(SaveConstants.WalletSaveKey, startValue);
        }
        
        public int CurrentValue { get; private set; }
        
        public void Add(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            CurrentValue += value;
            _view.Render(CurrentValue);
            
            _save.SetInt(SaveConstants.WalletSaveKey, CurrentValue);
        }

        public void Spend(int value)
        {
            if (CanSpend(value) == false)
                throw new InvalidOperationException();

            CurrentValue -= value;
            _view.Render(CurrentValue);
            
            _save.SetInt(SaveConstants.WalletSaveKey, CurrentValue);
        }

        public bool CanSpend(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            return CurrentValue - value >= 0;
        }
    }
}