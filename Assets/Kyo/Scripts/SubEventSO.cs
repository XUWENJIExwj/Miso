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
        static public string[] typeDesc = { "���C���C�x���g", "�T�u�C�x���g", "" };
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

        // Inspector�ɂ��鑮����ҏW����Ǝ��s�����R�[���o�b�N
        //void OnValidate()
        //{
        //    title = typeDesc[(int)type];
        //}
    }
}