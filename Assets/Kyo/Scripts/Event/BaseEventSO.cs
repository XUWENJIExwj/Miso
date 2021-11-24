using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventScriptableObject
{
    public enum BaseIndex
    {
        AR_02_Base,
        AT_01_Base,
        PC_01_Base,
        PC_05_Base,
        PC_07_Base,
        IN_02_Base,
        None,
    }

    public enum BaseEventPhase
    {
        Phase_Result,
        Phase_End,
        Phase_None,
    }

    [CreateAssetMenu(fileName = "Base_", menuName = "Base")]
    public class BaseEventSO : EventSO
    {
        public AMAs ama = AMAs.Max;
        public BaseEventPhase baseEventPhase = BaseEventPhase.Phase_None;

        protected override void Init()
        {
            InitEvent(EventSOType.Base);
        }

        public override void EventStart()
        {
            PollutionMap.instance.AddResult();

            BaseEventUI eventUI = EventUIManager.instance.GetCurrentEventUI<BaseEventUI>();
            eventUI.AppearResult();
            SetNextPhase(BaseEventPhase.Phase_Result);
        }

        public override void EventPlay()
        {
            switch(baseEventPhase)
            {
                case BaseEventPhase.Phase_Result:
                    Result();
                    break;
                case BaseEventPhase.Phase_End:
                    End();
                    break;
                default:
                    break;
            }
        }

        public override void ResetEventSO()
        {
            SetNextPhase(BaseEventPhase.Phase_None);
        }

        public void SetNextPhase(BaseEventPhase Phase)
        {
            baseEventPhase = Phase;
        }

        public void Result()
        {
            BaseEventUI eventUI = EventUIManager.instance.GetCurrentEventUI<BaseEventUI>();

            if (Input.GetMouseButtonDown(0) && !eventUI.ConfirmButtonPointerEnter())
            {
                if (eventUI.ResultTweenerActive())
                {
                    eventUI.ShowResult();
                }
            }
        }

        public void End()
        {
            ResetEventSO();
            RouteManager.instance.MovePath();
        }
    }
}