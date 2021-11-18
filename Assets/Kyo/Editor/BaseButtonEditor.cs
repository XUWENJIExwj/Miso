using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(BaseButton), true)]
[CanEditMultipleObjects]
public class BaseButtonEditor : EventButtonEditor
{
    private SerializedProperty baseIndex;

    public override void FindEventButtonProperty()
    {
        baseIndex = serializedObject.FindProperty("baseIndex");
    }

    public override void AddEventButtonPropertyField()
    {
        EditorGUILayout.PropertyField(baseIndex);
    }
}