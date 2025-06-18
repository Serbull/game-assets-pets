using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Serbull.GameAssets.Pets.Samples
{
    [Serializable]
    public class SaveData
    {
        public List<PetData> pets = new();
    }

    public class GameManager : MonoBehaviour
    {
        private SaveData _saveData;

        private void Start()
        {
            _saveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("saveData"));
            _saveData ??= new SaveData();

            PetManager.Initialize(_saveData.pets, "ru");

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
