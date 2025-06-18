using UnityEngine;

namespace Serbull.GameAssets.Pets
{
    public static class LocalizationProvider
    {
        private static string _language;
        private static PetConfig.LocalizationData[] _localizations;

        public static void Initialize(string language)
        {
            _language = language;
            _localizations = PetManager.Config.Localizations;
        }

        public static string GetText(string id)
        {
            foreach (var loc in _localizations)
            {
                if (loc.Id == id)
                {
                    return GetLocalization(loc);
                }
            }

            Debug.LogError("Not exist localization with id: " + id);
            return id;
        }

        private static string GetLocalization(PetConfig.LocalizationData localization)
        {
            return _language switch
            {
                "ru" => localization.Russian,
                _ => localization.English,
            };
        }

    }
}
