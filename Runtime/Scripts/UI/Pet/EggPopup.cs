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

        [SerializeField] private TextMeshProUGUI _priceText;

        [SerializeField] private GameObject _notEnoughtCup;
        [SerializeField] private GameObject _inventoryFull;

        private string _eggId;

        private void Awake()
        {
            _closeButton.onClick.AddListener(CloseButton_OnClick);
            _buyButton.onClick.AddListener(BuyButton_OnClick);
        }

        public void Init(string eggId)
        {
            Debug.LogError("Check");
            //ClearContent();

            //_eggId = eggId;
            //var petConfig = PetManager.Config;
            //var eggData = petConfig.GetEggData(eggId);
            //_notEnoughtCup.SetActive(false);

            //for (int i = 0; i < eggData.PetPobabilities.Length; i++)
            //{
            //    var slot = Instantiate(_eggSlotPrefab, _content);
            //    var icon = petConfig.GetPet(eggData.PetPobabilities[i].PetId).Icon;
            //    slot.Init(petConfig.GetPetRare(eggData.PetPobabilities[i].PetId), eggData.PetPobabilities[i].Pobability, icon);
            //}

            //_priceText.text = eggData.Price.ToShortValue();
            //_priceText.color = SaveManager.Data.money < petConfig.GetEggData(_eggId).Price ? Color.red : Color.white;
            //_inventoryFull.SetActive(petConfig.IsInventoryFull());
        }

        private void ClearContent()
        {
            foreach (Transform child in _content)
            {
                Destroy(child.gameObject);
            }
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
