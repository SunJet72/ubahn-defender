#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(ChildScrollRect))]
public class ChildScrollRectEditor : ScrollRectEditor
{
    SerializedProperty parentScrollRect;
    protected override void OnEnable()
    {
        base.OnEnable();
        parentScrollRect = serializedObject.FindProperty("parentScrollRect");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(parentScrollRect);
        serializedObject.ApplyModifiedProperties();
    }
}

#endif