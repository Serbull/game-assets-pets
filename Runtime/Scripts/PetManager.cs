using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Serbull.GameAssets.Pets
{
    public static class PetManager
    {
        public static Action OnEquippedPetChanged;
        public static Action OnPetAdded;

        private static PetConfig _config;
        private static List<PetData> _petSaveData;

        public static PetConfig Config
        {
            get
            {
                if (_config == null)
                {
                    _config = ConfigProvider.LoadConfig();
                }

                return _config;
            }
        }

        public static List<PetData> PetSaveData
        {
            get
            {
                if (_petSaveData == null)
                {
                    Debug.LogError("Use 'Initialize' method to assign save data");
                    return null;
                }

                return _petSaveData;
            }
        }

        public static void Initialize(List<PetData> petSaveData)
        {
            _petSaveData = petSaveData;
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

        public static void AddPet(string id)
        {
            PetSaveData.Add(new PetData { Id = id, IsEquipped = false });
            SortPets();
            OnPetAdded?.Invoke();
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

            var percent = UnityEngine.Mathf.Clamp(matchingPets.Count * 20, 0, 100);

            var success = UnityEngine.Random.Range(0, 100) < percent;

            var count = 0;
            foreach (var item in matchingPets)
            {
                if (count >= 5) break;
                _petSaveData.Remove(item);
                count++;
            }

            if (!success)
            {
                //Notification.ShowRed(LocalizationManager.GetText("merge_fail"));
                return;
            }

            //Notification.ShowGreen(LocalizationManager.GetText("merge_success"));

            _petSaveData.Add(new PetData { Id = id, IsGold = true });
        }

        public static void SetTheBest()
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
                    totalBonus += Config.GetPetData(pet.Id).GetBonus(pet.IsGold);
                }
            }

            return totalBonus;
        }

        public static void RemoveAllPetsExeptPurchased()
        {
            PetSaveData.RemoveAll(pet =>
            {
                var petData = Config.GetPetData(pet.Id);
                return petData != null && !petData.IsInApp;
            });

            SortPets();
            OnEquippedPetChanged?.Invoke();
        }

        public static bool IsInventoryFull()
        {
            return PetSaveData.Count >= Config.InventoryCapacity;
        }

        private static void SortPets()
        {
            PetSaveData.Sort((x, y) =>
            {
                var xBonus = Config.GetPetData(x.Id).GetBonus(x.IsGold);
                var yBonus = Config.GetPetData(y.Id).GetBonus(y.IsGold);
                return yBonus.CompareTo(xBonus);
            });
        }
    }
}
