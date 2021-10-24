using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventScriptableObject
{
    [CreateAssetMenu(fileName = "Base_", menuName = "Base")]
    public class BaseSO : EventSO
    {
        public Vector2Int pos;

        protected override void Init()
        {
            InitEvent(EventButtonType.Base);
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