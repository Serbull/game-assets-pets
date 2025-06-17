using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Serbull.GameAssets.Pets
{
    public class PetConfig : ScriptableObject
    {
        public int InventoryCapacity = 50;

        [Serializable]
        public class RareData
        {
            public string Id;
            public Color Color;
        }

        [Serializable]
        public class PetData
        {
            public string Id;
            [PetRareDropdown] public string Rare = "common";
            public float Bonus = 0.2f;
            public Sprite Icon;
            public Pet Prefab;
            public bool Mergable;
            public bool Undeletable;
            public bool IsInApp;
        }

        [Serializable]
        public class PetPobability
        {
            [PetDropdown] public string PetId;
            public int Pobability;
        }

        [Serializable]
        public class EggsData
        {
            public string Id;
            public long Price;
            public PetPobability[] PetPobabilities;
        }

        public RareData[] Rares;
        public PetData[] Pets;
        public EggsData[] EggsDatas;


        public EggsData GetEggData(string id)
        {
            foreach (var data in EggsDatas)
            {
                if (data.Id == id)
                {
                    return data;
                }
            }

            return null;
        }

        public List<int> GetEggIndexes(string id)
        {
            var eggData = GetEggData(id);
            if (eggData != null)
            {
                return eggData.PetPobabilities.Select(p => p.Pobability).ToList();
            }
            return new List<int>();
        }

        public float GetPetBonus(string petId, bool isGold)
        {
            for (int i = 0; i < Pets.Length; i++)
            {
                if (Pets[i].Id == petId)
                {
                    return Pets[i].Bonus * (isGold ? 2f : 1f);
                }
            }

            Debug.LogError($"Bonus not found for Pet ID: {petId}");
            return 0f;
        }

        public bool IsMergable(string id)
        {
            for (int i = 0; i < Pets.Length; i++)
            {
                if (Pets[i].Id == id)
                {
                    return Pets[i].Mergable;
                }
            }

            Debug.LogError($"Speed bonus not found for Pet ID: {id}");
            return false;
        }

        public bool IsUndelitable(string id)
        {
            for (int i = 0; i < Pets.Length; i++)
            {
                if (Pets[i].Id == id)
                {
                    return Pets[i].Undeletable;
                }
            }

            Debug.LogError($"Speed bonus not found for Pet ID: {id}");
            return false;
        }

        public RareData GetRareData(string id)
        {
            foreach (var rare in Rares)
            {
                if (rare.Id == id)
                {
                    return rare;
                }
            }

            Debug.LogError($"Not found rare with id: {id}");
            return null;
        }

        public int GetPetIndex(string id)
        {
            for (int i = 0; i < Pets.Length; i++)
            {
                if (Pets[i].Id == id)
                {
                    return i;
                }
            }

            return 0;
        }

        public PetData GetPetData(string petId)
        {
            foreach (var item in Pets)
            {
                if (item.Id == petId)
                {
                    return item;
                }
            }

            Debug.LogError($"Not found PetData with id: {petId}");
            return null;
        }

        public Pet GetRandomPet()
        {
            int randomIndex = Random.Range(0, Pets.Length);
            return Pets[randomIndex].Prefab;
        }

        public Pet GetRandomPet(string[] ids, out string id)
        {
            var availablePets = Pets.Where(p => ids.Contains(p.Id)).ToList();

            if (availablePets.Count == 0)
            {
                Debug.LogWarning("Нет доступных питомцев с указанными ID.");
                id = null;
                return null;
            }

            int randomIndex = Random.Range(0, availablePets.Count);
            id = availablePets[randomIndex].Id;
            return availablePets[randomIndex].Prefab;
        }
    }
}
