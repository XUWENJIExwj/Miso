using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EventScriptableObject
{
    [Serializable]
    public struct PointRange
    {
        public int min;
        public int max;
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

    public abstract class EventSO : ScriptableObject
    {
        [Header("Common")]
        public int id;
        public Sprite icon;
        [ReadOnly] public EventSOType type = EventSOType.None;
        static public string[] typeDesc = { "メインイベント", "サブイベント", "ランダムイベント", "ベース", "" };
        [ReadOnly] public string typeText = typeDesc[(int)EventSOType.None];
        public string eventTitle;
        public PointRange pointRange;
        public int point = 0;

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

        public abstract void ResetEventSO();

        public virtual void AddResult()
        {

        }

        public void SetPoint()
        {
            ComputePoint();
            SetPointText();

            Player.instance.AddPoint(point);
        }

        public virtual void ComputePoint()
        {
            point = UnityEngine.Random.Range(pointRange.min, pointRange.max);
        }

        public virtual void SetPointText()
        {
            
        }
    }
}