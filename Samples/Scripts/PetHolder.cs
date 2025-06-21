using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Serbull.GameAssets.Pets.Samples
{
    public class PetHolder : MonoBehaviour
    {
        private readonly List<Pet> _pets = new();

        [SerializeField] private float _distance = 1.3f;
        [SerializeField] private float _angle = 50f;

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

            var number = 0;

            var equippedCount = PetManager.PetSaveData.Count((p) => p.IsEquipped);

            foreach (var petData in PetManager.PetSaveData)
            {
                if (petData.IsEquipped)
                {
                    var prefab = PetManager.Config.GetPetData(petData.Id).Prefab;
                    var pet = Instantiate(prefab, transform);

                    var angle = (equippedCount - 1) * _angle / 2f - number * _angle;
                    pet.transform.localPosition = Quaternion.Euler(0, angle, 0) * (Vector3.back * _distance);
                    _pets.Add(pet);
                    number++;
                }
            }
        }
    }
}
