using UnityEngine;

namespace YellowSquad.Anthill.Meta
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private ShopButton _shopButtonTemplate;
        [SerializeField] private Transform _buttonsContent;
        
        public void Initialize(IWallet wallet, ShopButtonDTO[] buttons)
        {
            foreach (var button in buttons)
            {
                var buttonInstance = Instantiate(_shopButtonTemplate, _buttonsContent);
                buttonInstance.Initialize(button.ButtonName, button.ButtonCommand, button.PriceList, wallet);
            }
        }
    }
}
