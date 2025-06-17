using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Serbull.GameAssets.Pets
{
    public class PetSlot : MonoBehaviour
    {
        public Action<PetData> OnClicked;

        [SerializeField] private Image _petIcon;
        [SerializeField] private Image _background;
        [SerializeField] private TextMeshProUGUI _speedBonus;
        [SerializeField] private GameObject _equipMark;
        [SerializeField] private Sprite _goldSprite;
        [SerializeField] private GameObject _goldObj;

        private Button _button;
        private PetData _petData;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Button_OnClick);
        }

        public void Init(PetData petData, Sprite icon, Color backgroundColor, string energyBonus)
        {
            _petData = petData;
            _petIcon.sprite = icon;
            _background.color = backgroundColor;
            _speedBonus.text = energyBonus;
            _equipMark.SetActive(petData.IsEquipped);
            _goldObj.SetActive(petData.IsGold);

            if (petData.IsGold)
            {
                _background.sprite = _goldSprite;
            }
        }

        public void Button_OnClick()
        {
            OnClicked?.Invoke(_petData);
        }
    }
}
