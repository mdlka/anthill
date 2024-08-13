using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YellowSquad.GamePlatformSdk;

namespace YellowSquad.Anthill.Meta
{
    internal class UpgradeButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _upgradeInfoText;
        [SerializeField] private string _pricePrefix;
        [SerializeField] private LocalizedText[] _maxLocalization;

        private string _localizedMax;
        private IUpgrade _upgrade;

        internal Button Button => _button;

        public void Initialize(IUpgrade upgrade, string upgradeName, Sprite icon)
        {
            _upgrade = upgrade;
            _icon.sprite = icon;
            _nameText.text = upgradeName;

            _localizedMax = _maxLocalization.SelectCurrentLanguageText();
            
            RenderUpgradeInfo();
            _button.onClick.AddListener(OnButtonClick);
        }

        private void Update()
        {
            _button.interactable = _upgrade.CanPerform;
            _priceText.text = _upgrade.IsMax ? _localizedMax : $"{_pricePrefix}{_upgrade.CurrentPrice}";
            RenderUpgradeInfo();
        }

        private void OnButtonClick()
        {
            if (_upgrade.CanPerform == false)
                return;
            
            _upgrade.Perform();
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