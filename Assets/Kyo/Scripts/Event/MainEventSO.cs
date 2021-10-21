using UnityEngine;
using System;
using System.Collections.Generic;

namespace EventScriptableObject
{
    public enum CharacterType
    {
        Player,
        AMA,
        NPC,
        Aside,
        None,
    }

    public enum CompletedType
    {
        Failed,
        Succeeded,
        Special,
    }

    [Serializable]
    public struct TextObject
    {
        public CharacterType type;
        public Sprite character;
        public int textIndex;
        [TextArea(5, 20)] public string[] texts;
    }

    [Serializable]
    public struct EndingObject
    {
        public CompletedType type;
        [TextArea(5, 20)] public string endingText;
    }

    [Serializable]
    public struct MainEventOption
    {
        [TextArea(5, 20)] public string optionText;
        public int charcterIndex;
        public TextObject[] character;
        public EndingObject[] ending;
    }

    [Serializable]
    public struct ABNext
    {
        public TextObject[] next;
    }

    [CreateAssetMenu(fileName = "MainEvent_", menuName = "MainEvent")]
    public class MainEventSO : SubEventSO
    {
        [Header("MainEvent")]
        public TextObject ama;
        public MainEventOption[] ab;
        public ABNext[] abNext;
        public MainEventOption[] a;
        public MainEventOption[] b;

        protected override void Init()
        {
            InitEvent(EventButtonType.MainEvent);
        }

        public override void EventMovie()
        {

        }
    }
}