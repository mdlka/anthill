using TMPro;
using UnityEngine;

namespace YellowSquad.Anthill.Meta
{
    public class WalletView : MonoBehaviour, IWalletView
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private string _postfix;
        
        public void Render(int value)
        {
            _text.text = $"{value}{_postfix}";
        }
    }
}