using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Serbull.GameAssets.Pets.Samples
{
    [Serializable]
    public class SaveData
    {
        public List<PetData> Pets = new();
        public int Money = 1000;
    }

    public class Currency : ICurrency
    {
        private readonly SaveData _saveData;

        public Currency(SaveData saveData)
        {
            _saveData = saveData;
        }

        public long Amount => _saveData.Money;

        public void Add(long amount)
        {
            _saveData.Money += (int)amount;
        }

        public void Spend(long amount)
        {
            _saveData.Money -= (int)amount;
        }
    }

    public class GameManager : MonoBehaviour
    {
        public PetInstaller PetInstaller;

        public SaveData SaveData;
        public Currency TestCurrency;

        private void Awake()
        {
            SaveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("saveData"));
            SaveData ??= new SaveData();
            TestCurrency = new Currency(SaveData);

            PetInstaller.Init(TestCurrency, SaveData.Pets, "ru");

            Debug.Log("Use 'P' to add random pet.");
            Debug.Log("Use 'O' to add all pets.");
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                var pets = PetManager.PetConfig.Pets;
                var pet = pets[Random.Range(0, pets.Length)];
                PetManager.AddPet(pet.Id);
                Debug.Log("Added random pet: " + pet.Id);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                foreach (var pet in PetManager.PetConfig.Pets)
                {
                    PetManager.AddPet(pet.Id);
                }
                Debug.Log("Added all pets");
            }
        }
#endif
    }
}
