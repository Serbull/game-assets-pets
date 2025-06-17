using UnityEngine;
using System;

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
            [SerializeField] private float _bonus = 0.2f;
            public Sprite Icon;
            public Pet Prefab;
            public bool Mergable;
            public bool Undeletable;
            public bool IsInApp;

            public float GetBonus(bool isGold) => _bonus * (isGold ? 2f : 1f);
        }

        [Serializable]
        public class PetPobability
        {
            [PetDropdown] public string PetId;
            public int Pobability;
        }

        [Serializable]
        public class EggData
        {
            public string Id;
            public long Price;
            public PetPobability[] PetPobabilities;
        }

        public RareData[] Rares;
        public PetData[] Pets;
        public EggData[] Eggs;

        private void OnValidate()
        {
            foreach (var egg in Eggs)
            {
                var lastProbability = 100;
                for (int i = 0; i < egg.PetPobabilities.Length - 1; i++)
                {
                    lastProbability -= egg.PetPobabilities[i].Pobability;
                }
                egg.PetPobabilities[^1].Pobability = lastProbability;
            }
        }

        public RareData GetRareData(string rareId)
        {
            foreach (var rare in Rares)
            {
                if (rare.Id == rareId)
                {
                    return rare;
                }
            }

            Debug.LogError($"Not found RareData with id: {rareId}");
            return null;
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

        public EggData GetEggData(string eggId)
        {
            foreach (var data in Eggs)
            {
                if (data.Id == eggId)
                {
                    return data;
                }
            }

            Debug.LogError($"Not found EggData with id: {eggId}");
            return null;
        }
    }
}
