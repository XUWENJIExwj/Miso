using UnityEngine;

namespace EventScriptableObject
{
    [CreateAssetMenu(fileName = "MainEvent_", menuName = "MainEvent")]
    public class MainEventSO : SubEventSO
    {
        [Header("MainEvent")]
        public Sprite character;
        public string characterName;
        [TextArea(5, 20)] public string talkText;
        public Option[] options = new Option[2];

        protected override void Init()
        {
            InitEvent(EventButtonType.MainEvent);
        }

        public override void EventMovie()
        {

        }
    }
}