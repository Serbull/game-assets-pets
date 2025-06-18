using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        private string _lastShownEggId;

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

            Debug.LogError("Check");

            var petConfig = PetManager.Config;
            var eggData = petConfig.GetEggData(_currentEggId);

            for (int i = 0; i < eggData.PetPobabilities.Length; i++)
            {
                var slot = Instantiate(_eggSlotPrefab, _content);
                var petData = petConfig.GetPetData(eggData.PetPobabilities[i].PetId);
                slot.Init(petData.Icon, eggData.PetPobabilities[i].Pobability, petData.Rare);
            }

            _priceText.text = eggData.Price.ToShortValue();
            Debug.LogError("Check");
            //_priceText.color = SaveManager.Data.money < petConfig.GetEggData(_eggId).Price ? Color.red : Color.white;
            //_inventoryFull.SetActive(petConfig.IsInventoryFull());
        }

        private void OnDisable()
        {
            _lastShownEggId = _currentEggId;
            _currentEggId = null;
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
            Debug.LogError("Check");


            //var price = PetManager.Config.GetEggData(_eggId).Price;
            //if (SaveManager.Data.money < price)
            //{
            //    _notEnoughtCup.SetActive(true);
            //    return;
            //}

            //if (PetManager.IsInventoryFull())
            //{
            //    Debug.Log("Show notification inventory is full");
            //    return;
            //}

            //GameValues.SubstractEnergy(price);
            //new GiftRewardGiver().AddEgg(_eggId);
        }
    }
}
