using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Serbull.GameAssets.Pets
{
    public class EggSlot : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private TextMeshProUGUI _probabilityText;
        [SerializeField] private Image _petIcon;

        public void Init(Sprite icon, int probability, string rare)
        {
            _background.color = PetManager.Config.GetRareData(rare).Color;
            _probabilityText.text = probability.ToString() + "%";
            _petIcon.sprite = icon;
        }
    }
}
