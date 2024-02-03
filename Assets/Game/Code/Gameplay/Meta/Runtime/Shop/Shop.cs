using System;
using UnityEngine;
using YellowSquad.Anthill.Core.Ants;

namespace YellowSquad.Anthill.Meta
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private EditorShopButton _addDiggerButton;
        [SerializeField] private EditorShopButton _addLoaderButton;
        [SerializeField] private EditorShopButton _increaseSpeedButton;
        
        public void Initialize(IWallet wallet, ISession session, float minAntMoveDuration)
        {
            _addDiggerButton.Button.Initialize(
                new AddDiggerCommand(session), 
                new AlgebraicProgressionPriceList(_addDiggerButton.StartPrice, _addDiggerButton.AddingPriceValue), 
                wallet);
            
            _addLoaderButton.Button.Initialize(
                new AddLoaderCommand(session), 
                new AlgebraicProgressionPriceList(_addLoaderButton.StartPrice, _addLoaderButton.AddingPriceValue), 
                wallet);
            
            _increaseSpeedButton.Button.Initialize(
                new IncreaseSpeedCommand(session, 
                    new UpgradeAntMoveDurationList(_increaseSpeedButton.PricesCount, minAntMoveDuration, session.MaxAntMoveDuration)), 
                new LinearPriceList(_increaseSpeedButton.PricesCount, _increaseSpeedButton.StartPrice, _increaseSpeedButton.MaxPrice), 
                wallet);
        }

        [Serializable]
        private class EditorShopButton
        {
            [field: SerializeField] public ShopButton Button { get; private set; }
            [field: SerializeField, Min(0)] public int StartPrice { get; private set; }
            
            [field: Header("If endless prices")]
            [field: SerializeField, Min(0)] public int AddingPriceValue { get; private set; }
            
            [field: Header("Else")]
            [field: SerializeField, Min(0)] public int PricesCount { get; private set; }
            [field: SerializeField, Min(0)] public int MaxPrice { get; private set; }
        }
    }
}
