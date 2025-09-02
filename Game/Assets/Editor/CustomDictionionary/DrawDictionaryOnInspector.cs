using EconomicSystem;
using Scripts.Systems;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomDictionionary
{
    [CustomPropertyDrawer(typeof(SerializableDictionary<,>))]
    public class DrawDictionaryOnInspector : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.isExpanded = EditorGUI.Foldout(
                new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                property.isExpanded, label, true);

            if (!property.isExpanded)
                return;

            position.y += EditorGUIUtility.singleLineHeight;

            var keys = property.FindPropertyRelative("keys");
            var values = property.FindPropertyRelative("values");
            
            for (int i = 0; i < keys.arraySize; i++)
            {
                var keyProp = keys.GetArrayElementAtIndex(i);
                var valueProp = values.GetArrayElementAtIndex(i);

                float half = position.width / 2f;

                EditorGUI.PropertyField(
                    new Rect(position.x, position.y, half - 2, EditorGUIUtility.singleLineHeight),
                    keyProp, GUIContent.none);

                EditorGUI.PropertyField(
                    new Rect(position.x + half + 2, position.y, half - 2, EditorGUIUtility.singleLineHeight),
                    valueProp, GUIContent.none);

                position.y += EditorGUIUtility.singleLineHeight;
            }
            
            if (GUI.Button(new Rect(position.x, position.y, 60, EditorGUIUtility.singleLineHeight), "+"))
            {
                keys.arraySize++;
                values.arraySize++;
            }

            if (GUI.Button(new Rect(position.x + 65, position.y, 60, EditorGUIUtility.singleLineHeight), "-"))
            {
                if (keys.arraySize > 0)
                {
                    keys.arraySize--;
                    values.arraySize--;
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
                return EditorGUIUtility.singleLineHeight;

            var keys = property.FindPropertyRelative("keys");
            // foldout + список элементов + кнопки
            return (keys.arraySize + 2) * EditorGUIUtility.singleLineHeight;
        }
    }
}
