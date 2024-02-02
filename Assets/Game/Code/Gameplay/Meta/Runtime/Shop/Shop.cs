using System;
using UnityEngine;

namespace YellowSquad.Anthill.Meta
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private EditorShopButton _addDiggerButton;
        [SerializeField] private EditorShopButton _addLoaderButton;
        [SerializeField] private EditorShopButton _increaseSpeedButton;
        
        public void Initialize(IWallet wallet, ISession session)
        {
            _addDiggerButton.Button.Initialize(new AddDiggerCommand(session), 
                new AlgebraicProgressionPriceList(_addDiggerButton.StartPrice, _addDiggerButton.AddingPriceValue), 
                wallet);
            _addLoaderButton.Button.Initialize(new AddLoaderCommand(session), 
                new AlgebraicProgressionPriceList(_addLoaderButton.StartPrice, _addLoaderButton.AddingPriceValue), 
                wallet);
            _increaseSpeedButton.Button.Initialize(new IncreaseSpeedCommand(session), 
                new AlgebraicProgressionPriceList(_increaseSpeedButton.StartPrice, _increaseSpeedButton.AddingPriceValue), 
                wallet);
        }

        [Serializable]
        private class EditorShopButton
        {
            [field: SerializeField] public int StartPrice { get; private set; }
            [field: SerializeField] public int AddingPriceValue { get; private set; }
            [field: SerializeField] public ShopButton Button { get; private set; }
        }
    }
}
