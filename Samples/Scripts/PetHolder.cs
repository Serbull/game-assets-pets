using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Serbull.GameAssets.Pets.Samples
{
    public class PetHolder : MonoBehaviour
    {
        private enum UpdateType { Update, FixedUpdate }

        private readonly List<Transform> _pets = new();

        [SerializeField] private float _distance = 1.3f;
        [SerializeField] private float _angle = 50f;
        [SerializeField] private UpdateType _update = UpdateType.Update;
        [SerializeField] private float _moveLerpValue = 5f;

        private void Start()
        {
            PetManager.OnEquippedPetChanged += PetManager_OnEquippedPetChanged;
            PetManager_OnEquippedPetChanged();
        }

        private void OnDestroy()
        {
            PetManager.OnEquippedPetChanged -= PetManager_OnEquippedPetChanged;

            for (int i = 0; i < _pets.Count; i++)
            {
                if (_pets[i] != null)
                {
                    Destroy(_pets[i].gameObject);
                }
            }
        }

        private void Update()
        {
            if (_update != UpdateType.Update) return;

            UpdatePets(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (_update != UpdateType.FixedUpdate) return;

            UpdatePets(Time.fixedDeltaTime);
        }

        private void UpdatePets(float deltaTime)
        {
            for (int i = 0; i < _pets.Count; i++)
            {
                var angle = (_pets.Count - 1) * _angle / 2f - i * _angle;
                var targetPosition = transform.TransformPoint(Quaternion.Euler(0, angle, 0) * new Vector3(0, 0.1f, -_distance));
                _pets[i].SetPositionAndRotation(Vector3.Lerp(_pets[i].position, targetPosition, deltaTime * _moveLerpValue),
                    Quaternion.Lerp(_pets[i].rotation, transform.rotation, deltaTime * _moveLerpValue));
            }
        }

        private void PetManager_OnEquippedPetChanged()
        {
            foreach (var pet in _pets)
            {
                Destroy(pet.gameObject);
            }

            _pets.Clear();

            var equippedCount = PetManager.PetSaveData.Count((p) => p.IsEquipped);

            foreach (var petData in PetManager.PetSaveData)
            {
                if (petData.IsEquipped)
                {
                    var prefab = PetManager.Config.GetPetData(petData.Id).Prefab;
                    var pet = Instantiate(prefab, transform.position, transform.rotation);
                    _pets.Add(pet.transform);
                }
            }
        }
    }
}
