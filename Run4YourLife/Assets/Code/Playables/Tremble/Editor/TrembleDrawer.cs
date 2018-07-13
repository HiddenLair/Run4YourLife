using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TrembleBehaviour))]
public class TrembleDrawer : PropertyDrawer
{
    GUIContent m_TrembleConfig = new GUIContent("Tremble Configuration", "Scriptable object with the information for the camera tremble");
    GUIContent m_DurationUse = new GUIContent("Duration Use", "Check if wyou want this tremble to shake for the playable duration");

    public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
    {
        int fieldCount = 0;
        return fieldCount * EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty trembleConfig = property.FindPropertyRelative("trembleConfig");
        Rect singleFieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(singleFieldRect, trembleConfig, m_TrembleConfig);

        SerializedProperty durationUse = property.FindPropertyRelative("useDuration");
        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, durationUse, m_DurationUse);
    }
}
