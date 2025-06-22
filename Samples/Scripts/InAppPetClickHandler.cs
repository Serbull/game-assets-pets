using UnityEngine;

namespace Serbull.GameAssets.Pets.Samples
{
    public class InAppPetClickHandler : MonoBehaviour
    {
        private void OnMouseDown()
        {
            var pet = GetComponent<InAppPetStand>().PetId;
            FindAnyObjectByType<InappPetPopup>(FindObjectsInactive.Include).Show(pet);
        }
    }
}
