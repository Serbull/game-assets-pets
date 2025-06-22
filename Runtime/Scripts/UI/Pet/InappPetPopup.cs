using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Serbull.GameAssets.Pets
{
    public class InappPetPopup : MonoBehaviour
    {
        [SerializeField] private Image _petIcon;
        [SerializeField] private Image _glowImage;
        [SerializeField] private TextMeshProUGUI _petNameText;
        [SerializeField] private TextMeshProUGUI _petRareText;
        [SerializeField] private TextMeshProUGUI _petBonusText;
        [SerializeField] private Button _closeButton;

        private string _petId;

        private void Awake()
        {
            _closeButton.onClick.AddListener(CloseButton_OnClick);
        }

        private void OnEnable()
        {
            if (string.IsNullOrEmpty(_petId))
            {
                Debug.LogError($"For showing use 'Show' method.");
                return;
            }

            var petData = PetManager.Config.GetPetData(_petId);
            var rareData = PetManager.Config.GetRareData(petData.Rare);
            _petIcon.sprite = petData.Icon;
            _glowImage.color = rareData.Color;
            _petNameText.text = LocalizationProvider.GetText(_petId);
            _petRareText.text = LocalizationProvider.GetText(petData.Rare);
            _petRareText.color = rareData.Color;
            _petBonusText.text = "X" + petData.GetBonus(false);
        }

        private void OnDisable()
        {
            _petId = null;
        }

        public void Show(string petId)
        {
            _petId = petId;
            gameObject.SetActive(true);
        }

        private void CloseButton_OnClick()
        {
            gameObject.SetActive(false);
        }
    }
}
