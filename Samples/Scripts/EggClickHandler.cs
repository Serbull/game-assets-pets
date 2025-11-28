using UnityEngine;

namespace Serbull.GameAssets.Pets.Samples
{
    public class EggClickHandler : MonoBehaviour
    {
        private void OnMouseDown()
        {
            var eggId = GetComponent<EggStand>().EggId;
            PetManager.EggPopup.Show(eggId);
        }
    }
}
