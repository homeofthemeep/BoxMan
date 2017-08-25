using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(AxisKeys))]
public class AxisButtonDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        //dont indent
        int indentL = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        //Label
        EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        //Set position Rects
        Rect poslabel = new Rect(position.x, position.y, 15, position.height);
        Rect posField = new Rect(position.x + 75, position.y, 50, position.height);
        Rect neglabel = new Rect(position.x + 75, position.y, 15, position.height);
        Rect negField = new Rect(position.x + 145, position.y, 50, position.height);

        GUIContent posGUI = new GUIContent("+");
        GUIContent negGUI = new GUIContent("-");

        //Draw fields
        EditorGUI.LabelField(poslabel, posGUI);
        EditorGUI.PropertyField(posField, property.FindPropertyRelative("positive"), GUIContent.none);
        EditorGUI.LabelField(neglabel, negGUI);
        EditorGUI.PropertyField(negField, property.FindPropertyRelative("negative"), GUIContent.none);

        //reset indent
        EditorGUI.indentLevel = indentL;

        EditorGUI.EndProperty();
    }
}
