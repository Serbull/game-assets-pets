using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Serbull.GameAssets.Pets.Editor
{
    [CustomPropertyDrawer(typeof(PetDropdownAttribute))]
    public class PetDropdownDrawer : PropertyDrawer
    {
        private PetConfig _petConfig;

        private string[] _petIds;
        private string[] _petLabels;
        private Color[] _petColors;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CachePetData();

            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.LabelField(position, label.text, "Use [PetDropdown] with a string.");
                return;
            }

            if (_petIds == null || _petIds.Length == 0)
            {
                EditorGUI.LabelField(position, label.text, "No pets in config");
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            bool hasMixed = property.hasMultipleDifferentValues;
            EditorGUI.showMixedValue = hasMixed;

            int currentIndex = Array.IndexOf(_petIds, property.stringValue);
            if (currentIndex < 0) currentIndex = 0;

            var oldColor = GUI.contentColor;
            if (!hasMixed && currentIndex >= 0 && currentIndex < _petColors.Length)
                GUI.contentColor = _petColors[currentIndex];

            EditorGUI.BeginChangeCheck();
            int newIndex = EditorGUI.Popup(position, label.text, currentIndex, _petLabels);
            if (EditorGUI.EndChangeCheck())
            {
                property.stringValue = _petIds[newIndex];
            }

            GUI.contentColor = oldColor;
            EditorGUI.showMixedValue = false;

            EditorGUI.EndProperty();
        }

        private void CachePetData()
        {
            if (_petConfig == null)
            {
                _petConfig = ConfigProvider.LoadConfig();
            }

            if (_petConfig != null && _petConfig.Pets != null)
            {
                _petIds = _petConfig.Pets.Select(p => p.Id).ToArray();
                _petLabels = _petConfig.Pets.Select(p => $"{p.Id} [x{p.GetBonus(false)}]").ToArray();
                _petColors = _petConfig.Pets.Select(p => _petConfig.GetRareData(p.Rare).Color).ToArray();
            }
            else
            {
                _petIds = new string[0];
                _petLabels = new string[0];
                _petColors = new Color[0];
            }
        }
    }
}
