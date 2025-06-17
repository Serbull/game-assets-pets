using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Serbull.GameAssets.Pets
{
    public class PetPopup : MonoBehaviour
    {
        [SerializeField] private Transform _petContent;
        [SerializeField] private PetSlot _petSlot;

        [SerializeField] private Button _equipButton;
        [SerializeField] private Button _unequipButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _mergeButton;
        [SerializeField] private Button _equipTheBest;
        [SerializeField] private Button _removeButton;

        [Header("Main Pet")]
        [SerializeField] private GameObject _mainPetLayout;
        [SerializeField] private Image _mainPetImage;
        [SerializeField] private Image _mainPetBg;
        [SerializeField] private TextMeshProUGUI _mainPetNameText;

        [SerializeField] private TextMeshProUGUI _rareText;
        [SerializeField] private TextMeshProUGUI _equippedPetCountText;
        [SerializeField] private TextMeshProUGUI _petCountText;
        [SerializeField] private TextMeshProUGUI _bonusText;
        [SerializeField] private TextMeshProUGUI _specificPetText;
        [SerializeField] private LocalizationText _mergeText;
        [SerializeField] private LocalizationText _mergeChancheText;

        [SerializeField] private VertexGradient _goldPetColor;
        [SerializeField] private VertexGradient _premiumPetColor;

        private PetData _currentPetData;
        private string _lastSelectedId = null;

        private void Awake()
        {
            _equipButton.onClick.AddListener(EquipButton_OnClick);
            _unequipButton.onClick.AddListener(UnequipButton_OnClick);
            _closeButton.onClick.AddListener(CloseButton_OnClick);
            _mergeButton.onClick.AddListener(MergeButton_OnClick);
            _equipTheBest.onClick.AddListener(EquipTheBest_OnClick);
            _removeButton.onClick.AddListener(RemoveButton_OnClick);
        }

        private void OnEnable()
        {
            Init();
            UpdatePanel();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("Add random pet");
                var pets = PetManager.Config.Pets;
                var pet = pets[Random.Range(0, pets.Length)];
                PetManager.AddPet(pet.Id);
                Init();
            }
        }
#endif

        public void Init()
        {
            bool hasSelected = false;

            foreach (Transform child in _petContent)
            {
                Destroy(child.gameObject);
            }

            if (PetManager.PetSaveData.Count == 0)
            {
                _mergeButton.gameObject.SetActive(false);
                _removeButton.gameObject.SetActive(false);
                return;
            }

            foreach (PetData petSave in PetManager.PetSaveData)
            {
                var slot = Instantiate(_petSlot, _petContent);
                var petData = PetManager.Config.GetPetData(petSave.Id);
                var bonus = PetManager.Config.GetPetBonus(petSave.Id, petSave.IsGold);
                var rare = PetManager.Config.GetRareData(petData.Rare);

                slot.Init(petSave, petData.Icon, rare.Color, $"x{bonus.ToShortValue()}");
                slot.OnClicked += Slot_OnClicked;

                if (!hasSelected && petSave.Id == _lastSelectedId)
                {
                    hasSelected = true;
                    Slot_OnClicked(petSave);
                }
            }

            if (!hasSelected && PetManager.PetSaveData.Count > 0)
            {
                var firstPet = PetManager.PetSaveData.First();
                Slot_OnClicked(firstPet);
            }

            _equippedPetCountText.text = $"{PetManager.GetEqippedPets().Count}/{3}";
            _petCountText.text = $"{PetManager.PetSaveData.Count}/{PetManager.Config.InventoryCapacity}";
            _removeButton.gameObject.SetActive(_currentPetData != null && !_currentPetData.IsEquipped);
            UpdatePanel();

        }

        private void UpdatePanel()
        {
            _mainPetLayout.SetActive(_currentPetData != null);
        }

        private void Slot_OnClicked(PetData petData)
        {
            var petConfig = PetManager.Config;
            var pet = petConfig.GetPetData(petData.Id);
            var rare = petConfig.GetRareData(pet.Rare);
            _currentPetData = petData;

            _equipButton.gameObject.SetActive(!petData.IsEquipped);
            _unequipButton.gameObject.SetActive(petData.IsEquipped);

            var mergePetsCount = Mathf.Min(PetManager.GetSamePetsCount(petData.Id), 5);
            _mergeText.SetLocalizationId("merge", mergePetsCount);
            _mergeChancheText.SetLocalizationId("chance", mergePetsCount * 20);
            _bonusText.text = $"x{petConfig.GetPetBonus(petData.Id, petData.IsGold).ToShortValue()}";
            _removeButton.gameObject.SetActive(_currentPetData != null && !_currentPetData.IsEquipped && !petConfig.IsUndelitable(petData.Id));

            Debug.LogError("Check");
            //...text = LocalizationManager.GetText(rare.Id);
            _mainPetNameText.text = pet.Id;
            _rareText.text = rare.Id;
            _rareText.color = rare.Color;

            _mainPetImage.sprite = pet.Icon;
            _mainPetBg.color = rare.Color;

            bool isMergable = petConfig.IsMergable(petData.Id) && !_currentPetData.IsGold;
            _mergeButton.gameObject.SetActive(isMergable);

            if (_currentPetData.IsGold)
            {
                _specificPetText.gameObject.SetActive(true);
                _specificPetText.text = "gold";
                _specificPetText.colorGradient = _goldPetColor;
            }
            else if (petConfig.GetPetData(petData.Id).Undeletable)
            {
                _specificPetText.gameObject.SetActive(true);
                _specificPetText.text = "premium";
                _specificPetText.colorGradient = _premiumPetColor;
            }
            else
            {
                _specificPetText.gameObject.SetActive(false);
            }
        }

        private void CloseButton_OnClick()
        {
            gameObject.SetActive(false);
        }

        private void UnequipButton_OnClick()
        {
            PetManager.UnequipPet(_currentPetData.Id);

            Init();
        }

        private void EquipButton_OnClick()
        {
            if (_currentPetData == null)
            {
                return;
            }

            if (PetManager.GetEqippedPets().Count >= 3)
            {
                return;
            }

            PetManager.EquipPet(_currentPetData.Id);
            Init();
        }

        private void MergeButton_OnClick()
        {
            PetManager.Merge(_currentPetData.Id);
            Init();
        }

        private void EquipTheBest_OnClick()
        {
            PetManager.SetTheBest();
            Init();
        }

        private void RemoveButton_OnClick()
        {
            PetManager.RemovePet(_currentPetData.Id);
            Init();
        }
    }
}
