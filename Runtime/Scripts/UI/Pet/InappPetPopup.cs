using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Serbull.GameAssets.Pets
{
    public class InappPetPopup : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        //        [SerializeField] private YaGamesSDK.Components.IAPButton _buyButton;
        [HideInInspector] public string _petId;

        [SerializeField] private Image _petIcon;
        [SerializeField] private TextMeshProUGUI _valueTxt;

        private void Awake()
        {
            _closeButton.onClick.AddListener(CloseButton_OnClick);
        }

        private void CloseButton_OnClick()
        {
            gameObject.SetActive(false);
        }

        public void Init(Sprite icon, string value, string petId)
        {
            _petIcon.sprite = icon;
            _valueTxt.text = "X" + value;
            _petId = petId;
            SetId(_petId);
        }

        public void SetId(string petId)
        {
            _petId = petId;
            //_buyButton.ProductId = petId;
        }

        public void AddReward()
        {
            Debug.LogError("CHECK");
            //var petConfig = PetManager.Config;
            //PetManager.AddPet(_petId);

            //var petRare = petConfig.GetPetRare(_petId);
            //List<RewardSlotData> reward = new() { new RewardSlotData { Rare = petRare, Sprite = petConfig.GetPet(_petId).Icon } };
            //UIManager.Instance.RewardPreviewPopup.Show(reward);
        }
    }
}
