using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Serbull.GameAssets.Pets.Editor
{
    [CustomPropertyDrawer(typeof(PetDropdownAttribute))]
    public class PetDropdownDrawer : PropertyDrawer
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
                EditorGUI.LabelField(position, label.text, "Use [PetDropdown] with a string.");
            }
        }

        private string[] GetPetIds()
        {
            var config = PetManager.Config;
            if (config != null && config.Pets != null)
            {
                return config.Pets
                    //.Where(data => data._prefab != null)
                    .Select(data => data.Id)
                    .Distinct()
                    .ToArray();
            }
            return new string[0];
        }
    }
}
