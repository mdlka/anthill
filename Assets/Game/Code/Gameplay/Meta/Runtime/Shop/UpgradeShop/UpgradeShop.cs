using UnityEngine;

namespace YellowSquad.Anthill.Meta
{
    public class UpgradeShop : MonoBehaviour
    {
        [SerializeField] private UpgradeButton _upgradeButtonTemplate;
        [SerializeField] private Transform _buttonsContent;
        
        public void Initialize(UpgradeButtonDTO[] buttons)
        {
            foreach (var dto in buttons)
            {
                var buttonInstance = Instantiate(_upgradeButtonTemplate, _buttonsContent);
                buttonInstance.Initialize(dto.Upgrade, dto.ButtonName, dto.Icon);
            }
        }
    }
}
