using UnityEngine;
using System;

namespace EventSO
{
    [Serializable]
    public struct Option
    {
        public string optionText;
        public int maxPoint;
        public int minPoint;
    }

    [Serializable]
    public enum EventType
    {
        MainEvent,
        SubEvent,
        None,
    }

    [CreateAssetMenu(fileName = "SubEvent_", menuName = "SubEvent")]
    public class SubEventSO : ScriptableObject
    {
        [Header("Common")]
        public int id;
        [ReadOnly] public EventType type = EventType.None;
        static public string[] typeDesc = { "メインイベント", "サブイベント", "" };
        [ReadOnly] public string title = typeDesc[(int)EventType.None];
        [TextArea(5, 20)] public string eventText;

        void Awake()
        {
            InitEvent(EventType.SubEvent);
        }

        protected void InitEvent(EventType Type)
        {
            type = Type;
            title = typeDesc[(int)type];
        }

        // Inspectorにある属性を編集すると実行されるコールバック
        //void OnValidate()
        //{
        //    title = typeDesc[(int)type];
        //}
    }
}