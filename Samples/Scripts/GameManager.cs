using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using TMPro;

namespace Serbull.GameAssets.Pets.Samples
{
    [Serializable]
    public class SaveData
    {
        public List<PetData> Pets = new();
        public int Money = 1000;
    }

    public class GameManager : MonoBehaviour
    {
        public EggPopup EggPopup;

        public UnityEngine.UI.Button ButtonPrefab;
        public Transform ButtonsParent;

        private SaveData _saveData;
        private Money _money;

        private void Start()
        {
            _saveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("saveData"));
            _saveData ??= new SaveData();
            _money = new Money(_saveData);

            PetManager.Initialize(_saveData.Pets, "ru");

            InitializeEggButtons();

            Debug.Log("Use 'P' to add random pet.");
            Debug.Log("Use 'O' to add all pets.");
        }

        private void InitializeEggButtons()
        {
            for (int i = 0; i < PetManager.Config.Eggs.Length; i++)
            {
                var eggData = PetManager.Config.Eggs[i];
                var btn = Instantiate(ButtonPrefab, ButtonsParent);
                btn.onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                btn.onClick.AddListener(() => EggPopup.Show(eggData.Id, _money));
                btn.GetComponentInChildren<TextMeshProUGUI>().text = eggData.Id;
            }
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
