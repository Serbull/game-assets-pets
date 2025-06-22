using System;

namespace Serbull.GameAssets.Pets
{
    public class EggHatchPreviewPopup : Singleton<EggHatchPreviewPopup>
    {
        private Action _onHideCallback;

        private void OnEnable()
        {
            Invoke(nameof(Hide), 1.5f);
        }

        public void Show(Action onHideCallback)
        {
            _onHideCallback = onHideCallback;
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
            _onHideCallback?.Invoke();
        }
    }
}
