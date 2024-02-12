using UnityEngine;

namespace YellowSquad.Anthill.Meta
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private UpgradeButton _upgradeButtonTemplate;
        [SerializeField] private Transform _buttonsContent;
        
        public void Initialize(IWallet wallet, UpgradeButtonDTO[] buttons)
        {
            foreach (var dto in buttons)
            {
                var buttonInstance = Instantiate(_upgradeButtonTemplate, _buttonsContent);
                buttonInstance.Initialize(dto, wallet);
            }
        }
    }
}
