using UnityEngine;

namespace EventScriptableObject
{
    [CreateAssetMenu(fileName = "SubEvent_", menuName = "SubEvent")]
    public class SubEventSO : EventSO
    {
        [TextArea(5, 20)] public string eventSummary;

        protected override void Init()
        {
            InitEvent(EventButtonType.SubEvent);
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
    }
}