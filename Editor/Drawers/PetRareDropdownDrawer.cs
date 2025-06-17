using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Serbull.GameAssets.Pets.Editor
{
    [CustomPropertyDrawer(typeof(PetRareDropdownAttribute))]
    public class PetRareDropdownDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var petIds = GetPetIds();

            if (property.propertyType == SerializedPropertyType.String)
            {
                int currentIndex = Array.IndexOf(petIds, property.stringValue);
                if (currentIndex == -1) currentIndex = 0;

                int newIndex = EditorGUI.Popup(position, label.text, currentIndex, petIds);
                property.stringValue = petIds[newIndex];
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use [PetIdDropdown] with a string.");
            }
        }

        private string[] GetPetIds()
        {
            var config = PetManager.Config;
            if (config != null && config.Pets != null)
            {
                return config.Rares.Select(data => data.Id).ToArray();
            }
            return new string[0];
        }
    }
}
