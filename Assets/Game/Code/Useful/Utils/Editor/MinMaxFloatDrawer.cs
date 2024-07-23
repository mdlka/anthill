using UnityEditor;
using UnityEngine;

namespace YellowSquad.Utils.Editor
{
    [CustomPropertyDrawer(typeof(MinMaxFloat), true)]
    public class MinMaxFloatDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            EditorGUI.indentLevel = 0; // PropertyDrawer Indent fix for nested inspectors

            var minProperty = property.FindPropertyRelative("_min");
            var maxProperty = property.FindPropertyRelative("_max");

            const string minMaxLabel = "Min : Max";
            const int labelWidth = 58;
            const int spaceWidth = 4;
            
            float width = position.width / 2 - labelWidth / 2f - spaceWidth * 4;

            position.width = width;
            EditorGUI.PropertyField(position, minProperty, GUIContent.none);

            position.x += width + spaceWidth;
            position.width = labelWidth;
            EditorGUI.LabelField(position, minMaxLabel);

            position.x += labelWidth + spaceWidth;
            position.width = width;
            EditorGUI.PropertyField(position, maxProperty, GUIContent.none);

            if (GUI.changed)
            {
                if (maxProperty.floatValue < minProperty.floatValue) 
                    maxProperty.floatValue = minProperty.floatValue;

                property.serializedObject.ApplyModifiedProperties();
            }

            EditorGUI.EndProperty();
        }
    }
}
