using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Serbull.GameAssets.Pets
{
    public class PetManager : IPetService
    {
        public static event Action OnEquippedPetChanged;
        public static event Action OnPetAdded;

        private readonly ICurrency _eggShopCurrency;
        public ICurrency EggShopCurrency => _eggShopCurrency;

        public PetManager(ICurrency eggShopCurrency)
        {
            _eggShopCurrency = eggShopCurrency;
        }

        private static PetConfig _petConfig;
        private static List<PetData> _petSaveData;

        //UI
        private static PetPopup _petPopup;
        private static InappPetPopup _inappPetPopup;
        private static EggPopup _eggPopup;
        private static EggHatchPreviewPopup _eggHatchPreviewPopup;

        private static bool _autoEquipBestPets;

        public static void Init(PetConfig petConfig, List<PetData> petSaveData,
            PetPopup petPopup, InappPetPopup inappPetPopup,
            EggPopup eggPopup, EggHatchPreviewPopup eggHatchPreviewPopup,
            bool autoEquipBestPets)
        {
            _petConfig = petConfig;
            _petSaveData = petSaveData;

            _petPopup = petPopup;
            _inappPetPopup = inappPetPopup;
            _eggPopup = eggPopup;
            _eggHatchPreviewPopup = eggHatchPreviewPopup;

            _autoEquipBestPets = autoEquipBestPets;
        }

        public static PetConfig PetConfig
        {
            get
            {
                if (_petConfig == null)
                {
                    Debug.LogError("Use PetInstaller.Init() to initialize.");
                    return null;
                }

                return _petConfig;
            }
        }

        public static List<PetData> PetSaveData
        {
            get
            {
                if (_petSaveData == null)
                {
                    Debug.LogError("Use PetInstaller.Init() to initialize.");
                    return null;
                }

                return _petSaveData;
            }
        }

        public static PetPopup PetPopup
        {
            get
            {
                if (_petPopup == null)
                {
                    Debug.LogError("Use PetInstaller.Init() to initialize.");
                    return null;
                }

                return _petPopup;
            }
        }

        public static InappPetPopup InappPetPopup
        {
            get
            {
                if (_inappPetPopup == null)
                {
                    Debug.LogError("Use PetInstaller.Init() to initialize.");
                    return null;
                }

                return _inappPetPopup;
            }
        }

        public static EggPopup EggPopup
        {
            get
            {
                if (_eggPopup == null)
                {
                    Debug.LogError("Use PetInstaller.Init() to initialize.");
                    return null;
                }

                return _eggPopup;
            }
        }

        public static EggHatchPreviewPopup EggHatchPreviewPopup
        {
            get
            {
                if (_eggHatchPreviewPopup == null)
                {
                    Debug.LogError("Use PetInstaller.Init() to initialize.");
                    return null;
                }

                return _eggHatchPreviewPopup;
            }
        }

        IPetService.PetData IPetService.GetPetData(string petId)
        {
            var petData = PetConfig.GetPetData(petId);
            return new IPetService.PetData()
            {
                title = petData.Title,
                icon = petData.Icon,
                rarity = petData.Rarity
            };
        }

        public static void EquipPet(string id)
        {
            var pet = PetSaveData.FirstOrDefault(p => p.Id == id && !p.IsEquipped);

            if (pet != null)
            {
                pet.IsEquipped = true;
                SortPets();
                OnEquippedPetChanged?.Invoke();
            }
        }

        public static void UnequipPet(string id)
        {
            var pet = PetSaveData.FirstOrDefault(p => p.Id == id && p.IsEquipped);

            if (pet != null)
            {
                pet.IsEquipped = false;
                SortPets();
                OnEquippedPetChanged?.Invoke();
            }
        }

        public void AddEggWithPreview(string id)
        {
            if (PetConfig == null)
                return;

            var eggData = _petConfig.Eggs.FirstOrDefault(i => i.Id == id);
            if (eggData == null)
            {
                Debug.LogError($"Not exist Egg with id: {id}");
                return;
            }

            var weights = eggData.Pets.Select(i => i.Weight).ToArray();
            var index = MathfUtils.GetRandomIndexByWeight(weights);
            var petId = eggData.Pets[index].PetId;
            AddPet(petId);

            if (!EggHatchPreviewPopup.Instance)
            {
                Debug.LogError("Add 'EggHatchPreviewPopup.prefab' on the scene.");
                return;
            }

            EggHatchPreviewPopup.Instance.Show(() => Services.Reward.PreviewPet(petId));
        }

        public void AddPet(string id)
        {
            PetManager.AddPet(id);
        }

        private static void AddPet(string id, bool isGold = false)
        {
            PetSaveData.Add(new PetData { Id = id, IsGold = isGold });
            SortPets();
            OnPetAdded?.Invoke();

            if (_autoEquipBestPets)
            {
                EquipBestPets();
            }
        }

        public static void RemovePet(string id)
        {
            var petToRemove = PetSaveData.FirstOrDefault(p => p.Id == id && !p.IsEquipped);

            if (petToRemove != null)
            {
                PetSaveData.Remove(petToRemove);
                SortPets();
            }
        }

        public static void Merge(string id)
        {
            var matchingPets = PetSaveData.FindAll(p => p.Id == id && !p.IsGold);

            var percent = Mathf.Clamp(matchingPets.Count * 20, 0, 100);

            var success = UnityEngine.Random.Range(0, 100) < percent;

            var count = 0;
            var hasEquippedPets = false;

            foreach (var item in matchingPets)
            {
                if (count >= 5) break;

                if (!hasEquippedPets && item.IsEquipped)
                {
                    hasEquippedPets = true;
                }

                _petSaveData.Remove(item);
                count++;
            }

            if (hasEquippedPets)
            {
                OnEquippedPetChanged?.Invoke();
            }

            if (!success)
            {
                Services.UI.Notification.ShowRed(Services.Localization.GetText("merge_fail"));
                return;
            }

            Services.UI.Notification.ShowGreen(Services.Localization.GetText("merge_success"));

            AddPet(id, true);
        }

        public static void EquipBestPets()
        {
            if (PetSaveData.Count == 0) return;

            foreach (var pet in PetSaveData)
            {
                pet.IsEquipped = false;
            }

            SortPets();

            for (int i = 0; i < MathF.Min(PetSaveData.Count, 3); i++)
            {
                PetSaveData[i].IsEquipped = true;
            }

            OnEquippedPetChanged?.Invoke();
        }

        public static List<string> GetEqippedPets()
        {
            return PetSaveData.Where(p => p.IsEquipped).Select(p => p.Id).ToList();
        }

        public static int GetSamePetsCount(string id)
        {
            return PetSaveData.Count(p => p.Id == id && !p.IsGold);
        }

        public static float GetEquippedPetsBonus()
        {
            var totalBonus = 0f;

            for (int i = 0; i < PetSaveData.Count; i++)
            {
                var pet = PetSaveData[i];
                if (pet.IsEquipped)
                {
                    totalBonus += PetConfig.GetPetData(pet.Id).GetBonus(pet.IsGold);
                }
            }

            return totalBonus;
        }

        public static void RemoveAllPetsExeptPurchased()
        {
            PetSaveData.RemoveAll(pet =>
            {
                var petData = PetConfig.GetPetData(pet.Id);
                return petData != null && !petData.IsInApp;
            });

            SortPets();
            OnEquippedPetChanged?.Invoke();
        }

        public static bool IsInventoryFull()
        {
            return PetSaveData.Count >= PetConfig.InventoryCapacity;
        }

        private static void SortPets()
        {
            PetSaveData.Sort((x, y) =>
            {
                if (x.IsEquipped && !y.IsEquipped) return -1;
                else if (!x.IsEquipped && y.IsEquipped) return 1;
                else
                {
                    var xBonus = PetConfig.GetPetData(x.Id).GetBonus(x.IsGold);
                    var yBonus = PetConfig.GetPetData(y.Id).GetBonus(y.IsGold);
                    return yBonus.CompareTo(xBonus);
                }
            });
        }
    }
}
