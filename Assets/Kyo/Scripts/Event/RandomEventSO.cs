using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventScriptableObject
{
    [CreateAssetMenu(fileName = "RandomEvent_", menuName = "RandomEvent")]
    public class RandomEventSO : SubEventSO
    {
        protected override void Init()
        {
            InitEvent(EventSOType.RandomEvent);
        }

        public override void EventStart()
        {

        }

        public override void EventPlay()
        {
            // ‰¼
            RouteManager.instance.MovePath();
        }

        public override void ResetEventSO()
        {

        }

        public override void AddResult()
        {
            EventUIManager.instance.AddResult(this);
        }
    }
}
