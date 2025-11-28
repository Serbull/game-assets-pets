using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Serbull.GameAssets.Pets
{
    public class EggStand : MonoBehaviour
    {
        [SerializeField] private Image _currencyImage;
        [SerializeField] private TextMeshProUGUI _priceText;
        [Space]
        [SerializeField][EggDropdown] private string _eggId;

        public string EggId => _eggId;

        private void Start()
        {
            _currencyImage.sprite = PetManager.PetConfig.Visual.EggPriceSprite;
            _priceText.text = PetManager.PetConfig.GetEggData(_eggId).Price.ToShortValue();
        }
    }
}
