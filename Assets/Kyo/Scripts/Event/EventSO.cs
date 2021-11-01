using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EventScriptableObject
{
    [Serializable]
    public struct Option
    {
        public string optionText;
        [TextArea(5, 20)] public string nextEventText;
        public int maxPoint;
        public int minPoint;
    }

    [Serializable]
    public enum EventSOType
    {
        MainEvent,
        SubEvent,
        RandomEvent,
        Base,
        None,
    }

    [CreateAssetMenu(fileName = "Base_", menuName = "Base")]
    public abstract class EventSO : ScriptableObject
    {
        [Header("Common")]
        public int id;
        public Sprite icon;
        [ReadOnly] public EventSOType type = EventSOType.None;
        static public string[] typeDesc = { "メインイベント", "サブイベント", "ランダムイベント", "ベース", "" };
        [ReadOnly] public string typeText = typeDesc[(int)EventSOType.None];
        public string eventTitle;
        public int totalPoint;

        void Awake()
        {
            Init();
        }

        protected abstract void Init();

        protected void InitEvent(EventSOType Type)
        {
            type = Type;
            typeText = typeDesc[(int)type];
        }

        public abstract void EventStart();

        public abstract void EventPlay();

        public EventSOType GetEventType()
        {
            return type;
        }

        // Inspectorにある属性を編集するとEditorに反映してくれるコールバック
        //void OnValidate()
        //{
        //    title = typeDesc[(int)type];
        //}

        public abstract void ResetEventSO();

        public virtual void AddResult()
        {

        }
    }
}