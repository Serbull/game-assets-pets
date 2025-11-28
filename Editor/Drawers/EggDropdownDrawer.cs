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
            {
                CacheEggData();
            }

            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.LabelField(position, label.text, "Use [EggDropdown] with a string.");
                return;
            }

            if (_eggIds == null || _eggIds.Length == 0)
            {
                EditorGUI.LabelField(position, label.text, "No eggs in config");
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            // mixed mode
            bool hasMixed = property.hasMultipleDifferentValues;
            EditorGUI.showMixedValue = hasMixed;

            int currentIndex = Array.IndexOf(_eggIds, property.stringValue);
            if (currentIndex < 0) currentIndex = 0;

            EditorGUI.BeginChangeCheck();
            int newIndex = EditorGUI.Popup(position, label.text, currentIndex, _eggIds);
            if (EditorGUI.EndChangeCheck())
            {
                property.stringValue = _eggIds[newIndex];
            }

            EditorGUI.showMixedValue = false;

            EditorGUI.EndProperty();
        }

        private void CacheEggData()
        {
            var config = ConfigProvider.LoadConfig();
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
