using TMPro;
using UnityEngine;

namespace YellowSquad.Anthill.Meta.Map
{
    internal class MapCellPriceView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private string _prefix;
        
        public void Render(bool canBuyCell, int currentPrice)
        {
            _priceText.color = canBuyCell ? Color.green : Color.red;
            _priceText.text = $"{_prefix}{currentPrice.ToString()}";
        }
    }
}