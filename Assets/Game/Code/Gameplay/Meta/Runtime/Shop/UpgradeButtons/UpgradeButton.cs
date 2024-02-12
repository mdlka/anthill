using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YellowSquad.Anthill.Meta
{
    internal class UpgradeButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _upgradeInfoText;

        private IUpgrade _upgrade;
        private IPriceList _priceList;
        private IWallet _wallet;

        private bool CanClick => _upgrade.CanPerform && _wallet.CanSpend(_priceList.CurrentPrice);

        public void Initialize(UpgradeButtonDTO upgradeButtonDTO, IWallet wallet)
        {
            _upgrade = upgradeButtonDTO.Upgrade;
            _priceList = upgradeButtonDTO.PriceList;
            _wallet = wallet;

            _icon.sprite = upgradeButtonDTO.Icon;
            _nameText.text = upgradeButtonDTO.ButtonName;
            RenderUpgradeInfo();
                
            _button.onClick.AddListener(OnButtonClick);
        }

        private void Update()
        {
            _button.interactable = CanClick;
            _priceText.text = _priceList.HasNext && _upgrade.CanPerform ? $"Price: {_priceList.CurrentPrice}" : "Max";
        }

        private void OnButtonClick()
        {
            if (CanClick == false)
                return;
            
            _wallet.Spend(_priceList.CurrentPrice);
            _upgrade.Perform();
            _priceList.Next();

            RenderUpgradeInfo();
        }

        private void RenderUpgradeInfo()
        {
            _upgradeInfoText.text = $"{_upgrade.CurrentValue}/{_upgrade.MaxValue}";
        }
        
        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }
    }
}