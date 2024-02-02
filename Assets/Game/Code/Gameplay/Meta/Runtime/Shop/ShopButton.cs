using UnityEngine;
using UnityEngine.UI;

namespace YellowSquad.Anthill.Meta
{
    internal class ShopButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private IButtonCommand _command;
        private IPriceList _priceList;
        private IWallet _wallet;

        private bool CanClick => _command.CanExecute && _wallet.CanSpend(_priceList.CurrentPrice);

        public void Initialize(IButtonCommand command, IPriceList priceList, IWallet wallet)
        {
            _command = command;
            _priceList = priceList;
            _wallet = wallet;
            
            _button.onClick.AddListener(OnButtonClick);
        }

        private void Update()
        {
            _button.enabled = CanClick;
        }

        private void OnButtonClick()
        {
            if (CanClick == false)
                return;
            
            _wallet.Spend(_priceList.CurrentPrice);
            _command.Execute();
            _priceList.Next();
        }
        
        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }
    }
}