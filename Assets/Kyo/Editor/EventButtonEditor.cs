using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(EventButton), true)]
[CanEditMultipleObjects]
public class EventButtonEditor : ButtonEditor
{
    protected SerializedProperty gridPos;
    protected SerializedProperty isSelected;
    protected SerializedProperty eventSO;
    private SerializedProperty ocean;
    private SerializedProperty oceanArea;

    protected override void OnEnable()
    {
        base.OnEnable();
        gridPos = serializedObject.FindProperty("gridPos");
        isSelected = serializedObject.FindProperty("isSelected");
        eventSO = serializedObject.FindProperty("eventSO");
        FindEventButtonProperty();
    }

    public virtual void FindEventButtonProperty()
    {
        ocean = serializedObject.FindProperty("ocean");
        oceanArea = serializedObject.FindProperty("oceanArea");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        serializedObject.Update();
        EditorGUILayout.PropertyField(gridPos);
        EditorGUILayout.PropertyField(isSelected);
        EditorGUILayout.PropertyField(eventSO);
        AddEventButtonPropertyField();
        serializedObject.ApplyModifiedProperties();
    }

    public virtual void AddEventButtonPropertyField()
    {
        EditorGUILayout.PropertyField(ocean);
        EditorGUILayout.PropertyField(oceanArea);
    }
}