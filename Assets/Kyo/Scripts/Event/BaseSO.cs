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
    }
}