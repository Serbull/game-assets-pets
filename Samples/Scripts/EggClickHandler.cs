using UnityEngine;

namespace Serbull.GameAssets.Pets.Samples
{
    public class EggClickHandler : MonoBehaviour
    {
        private void OnMouseDown()
        {
            var eggId = GetComponent<EggStand>().EggId;
            GameManager.Instance.EggPopup.Show(eggId, new Money());
        }
    }
}
