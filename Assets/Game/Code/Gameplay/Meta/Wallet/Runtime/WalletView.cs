using TMPro;
using UnityEngine;

namespace YellowSquad.Anthill.Meta.Wallet
{
    public class WalletView : MonoBehaviour, IWalletView
    {
        [SerializeField] private TMP_Text _text;
        
        public void Render(int value)
        {
            _text.text = value.ToString();
        }
    }
}