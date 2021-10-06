using UnityEngine;

namespace EventScriptableObject
{
    [CreateAssetMenu(fileName = "SubEvent_", menuName = "SubEvent")]
    public class SubEventSO : EventSO
    {
        protected override void Init()
        {
            InitEvent(EventButtonType.SubEvent);
        }

        public override void EventMovie()
        {

        }
    }
}