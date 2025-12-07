using UnityEngine;
using UnityEditor;

namespace Serbull.GameAssets.Pets.Editor
{
    [CustomPropertyDrawer(typeof(PetConfig.EggData.Pet))]
    public class EggPetDrawer : PropertyDrawer
    {
        private GUIStyle _smallLabel;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _smallLabel ??= new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 12,
                fixedWidth = 12,
                margin = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(0, 0, 0, 0)
            };

            EditorGUI.BeginProperty(position, label, property);

            var petIdProp = property.FindPropertyRelative("PetId");
            var weightProp = property.FindPropertyRelative("Weight");

            // ⚠ тут берём бонус не из EggData.Pet, а из PetConfig.PetData
            var bonusProp = FindBonusProperty(property.serializedObject, petIdProp.stringValue);

            float h = EditorGUIUtility.singleLineHeight;

            int oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float x = position.x;
            float w = position.width;

            float petWidth = w * 0.55f;
            float signWidth = 14f;
            float bonusWidth = w * 0.20f;
            float weightWidth = w * 0.10f;

            // Pet dropdown
            var petRect = new Rect(x, position.y, petWidth, h);
            EditorGUI.PropertyField(petRect, petIdProp, GUIContent.none);
            x += petWidth + 2f;

            // x
            GUI.Label(new Rect(x, position.y, signWidth, h), "x", _smallLabel);
            x += signWidth + 2f;

            // Bonus (из PetData._bonus)
            var bonusRect = new Rect(x, position.y, bonusWidth, h);
            if (bonusProp != null)
            {
                EditorGUI.PropertyField(bonusRect, bonusProp, GUIContent.none);
            }
            else
            {
                // если не нашли PetData по Id — рисуем серое поле
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.FloatField(bonusRect, 0f);
                EditorGUI.EndDisabledGroup();
            }
            x += bonusWidth + 8f;

            // Weight (локальное поле яйца)
            var weightRect = new Rect(x, position.y, weightWidth, h);
            EditorGUI.PropertyField(weightRect, weightProp, GUIContent.none);
            x += weightWidth + 2f;

            // %
            GUI.Label(new Rect(x, position.y, signWidth, h), "%", _smallLabel);

            EditorGUI.indentLevel = oldIndent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        /// <summary>
        /// Ищем в PetConfig.Pets элемент с таким же Id и возвращаем его SerializedProperty "_bonus".
        /// </summary>
        private SerializedProperty FindBonusProperty(SerializedObject root, string petId)
        {
            if (string.IsNullOrEmpty(petId))
                return null;

            // поле в PetConfig: public PetData[] Pets;
            var petsArray = root.FindProperty("Pets");
            if (petsArray == null || !petsArray.isArray)
                return null;

            for (int i = 0; i < petsArray.arraySize; i++)
            {
                var element = petsArray.GetArrayElementAtIndex(i);
                var idProp = element.FindPropertyRelative("Id");
                if (idProp != null && idProp.stringValue == petId)
                {
                    return element.FindPropertyRelative("_bonus");
                }
            }

            return null;
        }
    }
}
