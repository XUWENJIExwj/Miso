using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(SingleSubEventAchievementButton), true)]
[CanEditMultipleObjects]
public class SingleSubEventAchievementButtonEditor : ButtonEditor
{
    protected SerializedProperty eventSO;
    protected SerializedProperty icon;
    protected SerializedProperty id;
    protected SerializedProperty titleFrame;
    protected SerializedProperty title;

    protected override void OnEnable()
    {
        base.OnEnable();
        eventSO = serializedObject.FindProperty("eventSO");
        icon = serializedObject.FindProperty("icon");
        id = serializedObject.FindProperty("id");
        titleFrame = serializedObject.FindProperty("titleFrame");
        title = serializedObject.FindProperty("title");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        serializedObject.Update();
        EditorGUILayout.PropertyField(eventSO);
        EditorGUILayout.PropertyField(icon);
        EditorGUILayout.PropertyField(id);
        EditorGUILayout.PropertyField(titleFrame);
        EditorGUILayout.PropertyField(title);
        AddPropertyField();
        serializedObject.ApplyModifiedProperties();
    }

    public virtual void AddPropertyField()
    {
        
    }
}
