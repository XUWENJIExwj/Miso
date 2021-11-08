using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(EventButton), true)]
[CanEditMultipleObjects]
public class EventButtonEditor : ButtonEditor
{
    private SerializedProperty gridPos;
    private SerializedProperty isSelected;
    private SerializedProperty eventSO;

    protected override void OnEnable()
    {
        base.OnEnable();
        gridPos = serializedObject.FindProperty("gridPos");
        isSelected = serializedObject.FindProperty("isSelected");
        eventSO = serializedObject.FindProperty("eventSO");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        serializedObject.Update();
        EditorGUILayout.PropertyField(gridPos);
        EditorGUILayout.PropertyField(isSelected);
        EditorGUILayout.PropertyField(eventSO);
        serializedObject.ApplyModifiedProperties();
    }
}