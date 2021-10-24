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
            InitEvent(EventButtonType.RandomEvent);
        }

        public override void EventMovie()
        {

        }
    }
}
