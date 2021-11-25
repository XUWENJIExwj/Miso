using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EventScriptableObject
{
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
        public AMATypes amaType = AMATypes.Type_None;
        public PointRange pointRange;
        public int point = 0;

        void Awake()
        {
            Init();
        }

        protected abstract void Init();

        protected virtual void InitEvent(EventSOType Type)
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
            CheckBouns();
            SetPointText();

            Player.instance.AddPoint(point);
        }

        public virtual void ComputePoint()
        {
            point = HelperFunction.RandomPointRange(pointRange);
        }

        public virtual void CheckBouns()
        {

        }

        public virtual void SetPointText()
        {
            
        }

        public bool IsRandomEvent()
        {
            return type == EventSOType.RandomEvent;
        }

        public virtual void MakePointRange()
        {

        }

        public virtual void StartAutoPlay()
        {

        }
    }
}