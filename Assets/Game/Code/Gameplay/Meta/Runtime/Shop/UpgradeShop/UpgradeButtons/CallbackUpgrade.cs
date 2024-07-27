using System;

namespace YellowSquad.Anthill.Meta
{
    public class CallbackUpgrade : IUpgrade
    {
        private readonly IUpgrade _upgrade;
        private readonly Action _callback;

        public CallbackUpgrade(IUpgrade upgrade, Action callback)
        {
            _upgrade = upgrade;
            _callback = callback;
        }

        public int MaxValue => _upgrade.MaxValue;
        public int CurrentValue => _upgrade.CurrentValue;
        public int CurrentPrice => _upgrade.CurrentPrice;
        public bool IsMax => _upgrade.IsMax;
        public bool CanPerform => _upgrade.CanPerform;
        
        public void Perform()
        {
            if (CanPerform == false)
                throw new InvalidOperationException();
            
            _upgrade.Perform();
            _callback?.Invoke();
        }
    }
}