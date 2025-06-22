using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Serbull.GameAssets.Pets
{
    public class InAppPetStand : MonoBehaviour
    {
        [SerializeField] private Image _currencyImage;
        [SerializeField] private TextMeshProUGUI _priceText;
        [Space]
        [SerializeField][PetDropdown] private string _petId;

        public string PetId => _petId;

        private void Start()
        {
            var petData = PetManager.Config.GetPetData(_petId);
            _currencyImage.sprite = PetManager.Config.Visual.BonusSprite;
            _priceText.text = "X" + petData.GetBonus(false).ToShortValue();
        }
    }
}
