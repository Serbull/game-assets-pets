using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Serbull.GameAssets.Pets.Editor
{
    [CustomPropertyDrawer(typeof(PetDropdownAttribute))]
    public class PetDropdownDrawer : PropertyDrawer
    {
        private string[] _petIds;
        private string[] _petLabels;
        private Color[] _petColors;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_petIds == null)
                CachePetData();

            if (property.propertyType == SerializedPropertyType.String)
            {
                int currentIndex = Array.IndexOf(_petIds, property.stringValue);
                if (currentIndex == -1) currentIndex = 0;

                EditorGUI.BeginProperty(position, label, property);

                var oldColor = GUI.contentColor;
                GUI.contentColor = _petColors[currentIndex];

                int newIndex = EditorGUI.Popup(position, label.text, currentIndex, _petLabels);

                GUI.contentColor = oldColor;

                property.stringValue = _petIds[newIndex];

                EditorGUI.EndProperty();
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use [PetDropdown] with a string.");
            }
        }

        private void CachePetData()
        {
            var config = PetManager.Config;
            if (config != null && config.Pets != null)
            {
                _petIds = config.Pets.Select(p => p.Id).ToArray();
                _petLabels = config.Pets.Select(p => $"{p.Id} [x{ p.GetBonus(false)}]").ToArray();
                _petColors = config.Pets.Select(p => config.GetRareData(p.Rare).Color).ToArray();
            }
            else
            {
                _petIds = new string[0];
                _petLabels = new string[0];
                _petColors = new Color[0];
            }
        }

        //private string[] GetPetIds()
        //{
        //    var config = PetManager.Config;

        //    if (config == null || config.Pets == null) return new string[0];

        //    return config.Pets.Select(data => data.Id).ToArray();
        //}
    }
}
