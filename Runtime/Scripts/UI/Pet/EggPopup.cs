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

            _priceImage.sprite = PetManager.Config.Visual.EggPriceSprite;
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

            var petConfig = PetManager.Config;
            var eggData = petConfig.GetEggData(_currentEggId);

            for (int i = 0; i < eggData.Pets.Length; i++)
            {
                var slot = Instantiate(_eggSlotPrefab, _content);
                var petData = petConfig.GetPetData(eggData.Pets[i].PetId);
                slot.Init(petData.Icon, eggData.Pets[i].Weight, petData.Rare);
            }

            _priceText.text = eggData.Price.ToShortValue();
        }

        private void OnDisable()
        {
            _currentEggId = null;
        }

        public void Show(string eggId, ICurrency currency)
        {
            _currentEggId = eggId;
            _currency = currency;

            gameObject.SetActive(true);
        }

        private void CloseButton_OnClick()
        {
            gameObject.SetActive(false);
        }

        private void BuyButton_OnClick()
        {
            if (_currentEggId == null) return;

            var config = PetManager.Config.GetEggData(_currentEggId);
            if (config.Price > _currency.Amount)
            {
                Notification.Instance.ShowRed(LocalizationProvider.GetText("not_enough_money"));
                return;
            }

            if (PetManager.IsInventoryFull())
            {
                Notification.Instance.ShowRed(LocalizationProvider.GetText("inventory_full"));
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

            EggHatchPreviewPopup.Instance.Show(() => PreviewPet(petId));
        }

        private void PreviewPet(string petId)
        {
            if (!RewardPreviewPopup.Instance)
            {
                Debug.LogError("Add 'RewardPreviewPopup.prefab' on the scene.");
                return;
            }

            var petData = PetManager.Config.GetPetData(petId);
            var rareData = PetManager.Config.GetRareData(petData.Rare);

            var item = new RewardPreviewItem(LocalizationProvider.GetText(petId),
                LocalizationProvider.GetText(petData.Rare),
                petData.Icon, 1, true,
                Color.white, rareData.Color, rareData.Color);

            RewardPreviewPopup.Instance.Show(item);
        }
    }
}
