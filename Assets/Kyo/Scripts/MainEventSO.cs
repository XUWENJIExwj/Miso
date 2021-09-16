using UnityEngine;

namespace EventSO
{
    [CreateAssetMenu(fileName = "MainEvent_", menuName = "MainEvent")]
    public class MainEventSO : SubEventSO
    {
        [Header("MainEvent")]
        public Sprite character;
        [TextArea(5, 20)] public string talkText;
        public Option[] options = new Option[2];

        void Awake()
        {
            InitEvent(EventType.MainEvent);
        }
    }
}