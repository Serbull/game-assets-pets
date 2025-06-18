using UnityEngine;
using System.Collections.Generic;

namespace Serbull.GameAssets.Pets.Samples
{
    public class Player : MonoBehaviour
    {
        private readonly List<Pet> _pets = new();

        private void Start()
        {
            PetManager.OnEquippedPetChanged += PetManager_OnEquippedPetChanged;
        }

        private void OnDestroy()
        {
            PetManager.OnEquippedPetChanged -= PetManager_OnEquippedPetChanged;
        }

        private void PetManager_OnEquippedPetChanged()
        {
            foreach (var pet in _pets)
            {
                Destroy(pet.gameObject);
            }

            _pets.Clear();

            foreach (var petData in PetManager.PetSaveData)
            {
                if (petData.IsEquipped)
                {
                    var prefab = PetManager.Config.GetPetData(petData.Id).Prefab;
                    _pets.Add(Instantiate(prefab, transform));
                }
            }
        }
    }
}
