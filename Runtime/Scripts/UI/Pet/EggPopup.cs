using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Serbull.GameAssets.Pets
{
    public class EggPopup : Popup
    {
        [SerializeField] private Transform _content;

        [SerializeField] private EggSlot _eggSlotPrefab;

        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _buyButton;

        [SerializeField] private Image _priceImage;
        [SerializeField] private TextMeshProUGUI _priceText;

        private string _currentEggId;

        private void Awake()
        {
            _closeButton.onClick.AddListener(Hide);
            _buyButton.onClick.AddListener(BuyButton_OnClick);

            _priceImage.sprite = PetManager.PetConfig.Visual.EggPriceSprite;
        }

        private void OnEnable()
        {
            foreach (Transform child in _content)
            {
                Destroy(child.gameObject);
            }

            if (string.IsNullOrEmpty(_currentEggId))
            {
                Debug.LogError($"For showing use 'Show' method.");
                return;
            }

            var petConfig = PetManager.PetConfig;
            var eggData = petConfig.GetEggData(_currentEggId);

            for (int i = 0; i < eggData.Pets.Length; i++)
            {
                var slot = Instantiate(_eggSlotPrefab, _content);
                var petData = petConfig.GetPetData(eggData.Pets[i].PetId);
                slot.Init(petData.Icon, eggData.Pets[i].Weight, petData.Rarity);
            }

            _priceText.text = eggData.Price.ToShortValue();
        }

        private void OnDisable()
        {
            _currentEggId = null;
        }

        public void Show(string eggId)
        {
            _currentEggId = eggId;
            base.Show();
        }

        private void BuyButton_OnClick()
        {
            if (_currentEggId == null) return;

            var currency = Services.PetService.EggShopCurrency;
            var config = PetManager.PetConfig.GetEggData(_currentEggId);
            if (config.Price > currency.Amount)
            {
                Services.UI.Notification.ShowRed(Services.Localization.GetText("not_enough_money"));
                return;
            }

            if (PetManager.IsInventoryFull())
            {
                Services.UI.Notification.ShowRed(Services.Localization.GetText("inventory_full"));
                return;
            }

            currency.Spend(config.Price);
            Services.PetService.AddEggWithPreview(_currentEggId);
        }
    }
}
