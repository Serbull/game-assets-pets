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

    public class GameManager : Singleton<GameManager>
    {
        public EggPopup EggPopup;

        public SaveData SaveData;

        private void Start()
        {
            SaveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("saveData"));
            SaveData ??= new SaveData();

            PetManager.Initialize(SaveData.Pets, "ru");

            Debug.Log("Use 'P' to add random pet.");
            Debug.Log("Use 'O' to add all pets.");
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                var pets = PetManager.Config.Pets;
                var pet = pets[Random.Range(0, pets.Length)];
                PetManager.AddPet(pet.Id);
                Debug.Log("Added random pet: " + pet.Id);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                foreach (var pet in PetManager.Config.Pets)
                {
                    PetManager.AddPet(pet.Id);
                }
                Debug.Log("Added all pets");
            }
        }
#endif
    }
}
