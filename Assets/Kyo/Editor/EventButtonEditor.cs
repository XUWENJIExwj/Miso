using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(EventButton), true)]
[CanEditMultipleObjects]
public class EventButtonEditor : ButtonEditor
{
    protected SerializedProperty gridPos;
    protected SerializedProperty isSelected;
    protected SerializedProperty eventSO;
    protected SerializedProperty eventButtonUI;

    protected override void OnEnable()
    {
        base.OnEnable();
        gridPos = serializedObject.FindProperty("gridPos");
        isSelected = serializedObject.FindProperty("isSelected");
        eventSO = serializedObject.FindProperty("eventSO");
        FindButtonUIProperty();
    }

    public virtual void FindButtonUIProperty()
    {
        eventButtonUI = serializedObject.FindProperty("eventButtonUI");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        serializedObject.Update();
        EditorGUILayout.PropertyField(gridPos);
        EditorGUILayout.PropertyField(isSelected);
        EditorGUILayout.PropertyField(eventSO);
        EditorGUILayout.PropertyField(eventButtonUI);
        serializedObject.ApplyModifiedProperties();
    }
}