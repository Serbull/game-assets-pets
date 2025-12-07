using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using static Serbull.GameAssets.Pets.PetConfig.EggData;

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
            [SerializeField] private float _bonus = 1f;
            public string Title;
            public Sprite Icon;
            public Pet Prefab;
            public bool Mergable;
            public bool Undeletable;
            public bool IsInApp;

            public float GetBonus(bool isGold) => _bonus * (isGold ? 2f : 1f);
        }

        [Serializable]
        public class EggData
        {
            [Serializable]
            public class Pet
            {
                [PetDropdown] public string PetId;
                public int Weight;
            }

            public string Id;
            public long Price;
            public Pet[] Pets;
        }

        [Serializable]
        public class VisualData
        {
            public Sprite BonusSprite;
            public Sprite EggPriceSprite;
        }

        [Serializable]
        public class LocalizationData
        {
            public string Id;
            public string English;
            public string Russian;
        }

        public RareData[] Rares;
        public PetData[] Pets;
        public EggData[] Eggs;
        [Space]
        public LocalizationData[] Localizations;
        [Space]
        public VisualData Visual;

        private void OnValidate()
        {
            foreach (var egg in Eggs)
            {
                var lastProbability = 100;
                for (int i = 0; i < egg.Pets.Length - 1; i++)
                {
                    lastProbability -= egg.Pets[i].Weight;
                }
                egg.Pets[^1].Weight = lastProbability;
            }
        }

        [Button("Auto Generate Eggs (6 Pets in egg)", 10)]
        private void AutoEgg6Pets()
        {
            var allPets = new List<PetData>();
            var uniquePets = new List<string>();
            foreach (var pet in Pets)
            {
                if (!pet.IsInApp)
                {
                    allPets.Add(pet);

                    var id = pet.Id.Split("_")[0];
                    if (!uniquePets.Contains(id))
                    {
                        uniquePets.Add(id);
                    }
                }
            }

            var eggsCount = allPets.Count / 6;
            var eggs = new List<EggData>(eggsCount);
            var uniquePetsInEggs = new List<string>();
            var weights = new int[] { 45, 25, 15, 8, 5, 2 };

            for (int i = 0; i < eggsCount; i++)
            {
                var egg = new EggData() { Id = "egg_" + (i + 1) };

                var raresInEgg = new List<string>();
                var uniquePetsInEgg = new List<string>();

                var eggPets = new List<EggData.Pet>();

                for (int j = 0; j < allPets.Count; j++)
                {
                    if (uniquePetsInEggs.Count == uniquePets.Count)
                    {
                        uniquePetsInEggs.Clear();
                        j = 0;
                    }

                    var id = allPets[j].Id.Split("_")[0];

                    if (!raresInEgg.Contains(allPets[j].Rare) && !uniquePetsInEgg.Contains(id) && !uniquePetsInEggs.Contains(id))
                    {
                        raresInEgg.Add(allPets[j].Rare);
                        uniquePetsInEgg.Add(id);
                        uniquePetsInEggs.Add(id);
                        eggPets.Add(new EggData.Pet() { PetId = allPets[j].Id, Weight = weights[eggPets.Count] });
                        allPets.RemoveAt(j);
                    }
                }

                egg.Pets = eggPets.ToArray();

                eggs.Add(egg);
            }

            Eggs = eggs.ToArray();
        }

        [Button("Auto Generate Eggs (3 Pets in egg)")]
        private void AutoEgg3Pets()
        {
            var allPets = new List<PetData>();
            var uniquePets = new List<string>();
            foreach (var pet in Pets)
            {
                if (!pet.IsInApp)
                {
                    allPets.Add(pet);

                    var id = pet.Id.Split("_")[0];
                    if (!uniquePets.Contains(id))
                    {
                        uniquePets.Add(id);
                    }
                }
            }

            var eggsCount = allPets.Count / 6;
            var eggs = new List<EggData>(eggsCount);
            var uniquePetsInEggs = new List<string>();
            var weights = new int[] { 60, 30, 10 };

           
            for (int i = 0; i < eggsCount; i++)
            {
                var targetRares = (i + 1) % 2 == 1 ?
                    new string[] { "common","rare","legendary" } :
                    new string[] { "uncommon", "epic", "mystical" };

                var egg = new EggData() { Id = "egg_" + (i + 1) };

                var raresInEgg = new List<string>();
                var uniquePetsInEgg = new List<string>();

                var eggPets = new List<EggData.Pet>();

                for (int j = 0; j < allPets.Count; j++)
                {
                    if (uniquePetsInEggs.Count == uniquePets.Count)
                    {
                        uniquePetsInEggs.Clear();
                        j = 0;
                    }

                    var id = allPets[j].Id.Split("_")[0];

                    if (!raresInEgg.Contains(allPets[j].Rare) && !uniquePetsInEgg.Contains(id) && !uniquePetsInEggs.Contains(id) && targetRares.Contains(allPets[j].Rare))
                    {
                        raresInEgg.Add(allPets[j].Rare);
                        uniquePetsInEgg.Add(id);
                        uniquePetsInEggs.Add(id);

                        eggPets.Add(new EggData.Pet() { PetId = allPets[j].Id, Weight = weights[eggPets.Count] });
                        allPets.RemoveAt(j);
                    }
                }

                egg.Pets = eggPets.ToArray();

                eggs.Add(egg);
            }

            Eggs = eggs.ToArray();
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
