using UnityEngine;
using UnityEngine.UI;

namespace YellowSquad.Anthill.Meta
{
    public class UpgradeShop : MonoBehaviour
    {
        [SerializeField] private UpgradeButton _upgradeButtonTemplate;
        [SerializeField] private Transform _buttonsContent;
        
        public Button[] Initialize(UpgradeButtonDTO[] buttons)
        {
            var instances = new Button[buttons.Length];

            for (int i = 0; i < buttons.Length; i++)
            {
                var dto = buttons[i];
                var buttonInstance = Instantiate(_upgradeButtonTemplate, _buttonsContent);
                buttonInstance.Initialize(dto.Upgrade, dto.ButtonName, dto.Icon);

                instances[i] = buttonInstance.Button;
            }

            return instances;
        }
    }
}
