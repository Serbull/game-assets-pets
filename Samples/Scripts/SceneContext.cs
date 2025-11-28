using UnityEngine;

namespace Serbull.GameAssets.Pets.Samples
{
    public class SceneContext : MonoBehaviour
    {
        [SerializeField] private SGAInstaller _sgaInstaller;
        [SerializeField] private PetInstaller _petInstaller;

        public SaveData SaveData;
        public Currency Currency;

        private void Awake()
        {
            SaveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("saveData"));
            SaveData ??= new SaveData();
            Currency = new Currency(SaveData);

            var lang = "ru";

            _sgaInstaller.Init(null);
            _petInstaller.Init(Currency, SaveData.Pets, lang);
        }
    }
}
