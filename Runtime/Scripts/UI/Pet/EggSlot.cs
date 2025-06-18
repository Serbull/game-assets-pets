using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Serbull.GameAssets.Pets
{
    public class EggSlot : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private TextMeshProUGUI _rareText;
        [SerializeField] private TextMeshProUGUI _probabilityText;
        [SerializeField] private Image _petIcon;

        public void Init(Sprite icon, int probability, string rare)
        {
            Debug.LogError("Check");
            //var data = Configs.Instance.RareConfig.GetDatas(rare);
            //_background.color = data.Color;
            //_probabilityText.text = probability.ToString() + "%";
            //_rareText.text = LocalizationManager.GetText(data.RareName);
            //_petIcon.sprite = icon;
        }

        public void Init(Sprite icon)
        {
            _petIcon.sprite = icon;
            _rareText.gameObject.SetActive(false);
            _probabilityText.gameObject.SetActive(false);
        }
    }
}
