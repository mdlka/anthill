using TMPro;
using UnityEngine;

namespace YellowSquad.Anthill.Meta
{
    internal class MapCellPriceView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private string _prefix;
        
        public void Render(int currentPrice, Color color)
        {
            _priceText.color = color;
            _priceText.text = $"{_prefix}{currentPrice.ToString()}";
        }
    }
}