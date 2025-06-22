using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Serbull.GameAssets.Pets.Editor
{
    [CustomPropertyDrawer(typeof(EggDropdownAttribute))]
    public class EggDropdownDrawer : PropertyDrawer
    {
        private string[] _eggIds;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_eggIds == null)
                CacheEggData();

            if (property.propertyType == SerializedPropertyType.String)
            {
                int currentIndex = Array.IndexOf(_eggIds, property.stringValue);
                if (currentIndex == -1) currentIndex = 0;

                EditorGUI.BeginProperty(position, label, property);

                int newIndex = EditorGUI.Popup(position, label.text, currentIndex, _eggIds);

                property.stringValue = _eggIds[newIndex];

                EditorGUI.EndProperty();
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use [PetDropdown] with a string.");
            }
        }

        private void CacheEggData()
        {
            var config = PetManager.Config;
            if (config != null && config.Eggs != null)
            {
                _eggIds = config.Eggs.Select(p => p.Id).ToArray();
            }
            else
            {
                _eggIds = new string[0];
            }
        }
    }
}
