using UnityEngine;
using System.Collections.Generic;

namespace Serbull.GameAssets.Pets
{
    [ExecuteInEditMode]
    public class PetInstaller : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField, ReadOnly] private PetConfig _petConfig;
        [Header("UI")]
        [SerializeField] private PetPopup _petPopup;
        [SerializeField] private InappPetPopup _inappPetPopup;
        [SerializeField] private EggPopup _eggPopup;
        [SerializeField] private EggHatchPreviewPopup _eggHatchPreviewPopup;
        [Header("Settings")]
        [SerializeField] private bool _autoEquipBestPets = true;

#if UNITY_EDITOR
        private void OnEnable()
        {
            if (!Application.isPlaying)
            {
                _petConfig = ConfigProvider.LoadConfig();
            }
        }
#endif

        public void Init(ICurrency eggShopCurrency, List<PetData> saveData)
        {
            IPetService petService = new PetManager(eggShopCurrency);
            PetManager.Init(_petConfig, saveData,
                _petPopup, _inappPetPopup,
                _eggPopup, _eggHatchPreviewPopup, _autoEquipBestPets);

            Services.PetService = petService;

            Services.Localization.AddLocalization(_petConfig.Localizations);
        }
    }
}
