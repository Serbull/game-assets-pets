using UnityEngine;
using System;
using System.Collections.Generic;

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

            PetManager.Initialize(_saveData.pets);
        }
    }
}
