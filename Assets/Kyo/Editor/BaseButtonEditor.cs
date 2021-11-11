using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(BaseButton), true)]
[CanEditMultipleObjects]
public class BaseButtonEditor : EventButtonEditor
{
    public override void FindButtonUIProperty()
    {
        eventButtonUI = serializedObject.FindProperty("baseButtonUI");
    }
}