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

            var petData = PetManager.PetConfig.GetPetData(_petId);
            var rarityData = Services.Rarity.GetRarityData(petData.Rarity);
            _petIcon.sprite = petData.Icon;
            _glowImage.color = rarityData.Color;
            _petNameText.text = Services.Localization.GetText(_petId);
            _petRareText.text = Services.Localization.GetText(rarityData.LocalizationId);
            _petRareText.color = rarityData.Color;
            _petBonusText.text = "X" + petData.GetBonus(false).ToShortValue();
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
