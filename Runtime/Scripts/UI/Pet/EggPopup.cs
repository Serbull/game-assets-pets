using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Serbull.GameAssets.Pets
{
    public class EggPopup : MonoBehaviour
    {
        [SerializeField] private Transform _content;

        [SerializeField] private EggSlot _eggSlotPrefab;

        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _buyButton;

        [SerializeField] private Image _priceImage;
        [SerializeField] private TextMeshProUGUI _priceText;

        private string _currentEggId;
        private ICurrency _currency;

        private void Awake()
        {
            _closeButton.onClick.AddListener(CloseButton_OnClick);
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

        public void Init(ICurrency currency)
        {
            _currency = currency;
        }

        public void Show(string eggId)
        {
            _currentEggId = eggId;

            gameObject.SetActive(true);
        }

        private void CloseButton_OnClick()
        {
            gameObject.SetActive(false);
        }

        private void BuyButton_OnClick()
        {
            if (_currentEggId == null) return;

            var config = PetManager.PetConfig.GetEggData(_currentEggId);
            if (config.Price > _currency.Amount)
            {
                Services.UI.Notification.ShowRed(LocalizationProvider.GetText("not_enough_money"));
                return;
            }

            if (PetManager.IsInventoryFull())
            {
                Services.UI.Notification.ShowRed(LocalizationProvider.GetText("inventory_full"));
                return;
            }

            _currency.Spend(config.Price);

            var weights = config.Pets.Select((i) => i.Weight).ToArray();
            var id = MathfUtils.GetRandomIndexByWeight(weights);
            var petId = config.Pets[id].PetId;
            PetManager.AddPet(petId);

            if (!EggHatchPreviewPopup.Instance)
            {
                Debug.LogError("Add 'EggHatchPreviewPopup.prefab' on the scene.");
                return;
            }

            EggHatchPreviewPopup.Instance.Show(() => PetManager.PreviewPet(petId));
        }
    }
}
