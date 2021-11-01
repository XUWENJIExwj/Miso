using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(ResultConfirmButton), true)]
[CanEditMultipleObjects]
public class ResultConfirmButtonEditor : ButtonEditor
{
    private SerializedProperty pointerEnter;

    protected override void OnEnable()
    {
        base.OnEnable();
        pointerEnter = serializedObject.FindProperty("pointerEnter");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        serializedObject.Update();
        EditorGUILayout.PropertyField(pointerEnter);
        serializedObject.ApplyModifiedProperties();
    }
}
