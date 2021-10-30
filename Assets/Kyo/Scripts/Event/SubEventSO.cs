using UnityEngine;
using DG.Tweening;

namespace EventScriptableObject
{
    public enum SubEventPhase
    {
        Phase_ReportPre,
        Phase_Report,
        Phase_None,
    }

    [CreateAssetMenu(fileName = "SubEvent_", menuName = "SubEvent")]
    public class SubEventSO : EventSO
    {
        [TextArea(5, 20)] public string eventSummary;
        public SubEventPhase subEventPhase = SubEventPhase.Phase_None;
        private Tweener tweener = null;

        protected override void Init()
        {
            InitEvent(EventButtonType.SubEvent);
        }

        public override void EventStart()
        {
            SubEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<SubEventUI>().GetEventUIElement();
            ui.Title.text = eventTitle;
            ui.Summary.text = eventSummary;

            // ‰¼
            float time = 0.8f;
            tweener = ui.TitleFrame.DOFade(1.0f, time).OnUpdate(() =>
            {
                ui.Title.color = HelperFunction.ChangeAlpha(ui.Title.color, ui.TitleFrame.color.a);
                ui.Summary.color = HelperFunction.ChangeAlpha(ui.Summary.color, ui.TitleFrame.color.a);
            }).OnComplete(() =>
            {
                ui.Title.color = HelperFunction.ChangeAlpha(ui.Title.color, ui.TitleFrame.color.a);
                ui.Summary.color = HelperFunction.ChangeAlpha(ui.Summary.color, ui.TitleFrame.color.a);
                SetNextPhase(SubEventPhase.Phase_Report);
            });

            SetNextPhase(SubEventPhase.Phase_ReportPre);
        }

        public override void EventPlay()
        {
            switch (subEventPhase)
            {
                case SubEventPhase.Phase_ReportPre:
                    break;
                case SubEventPhase.Phase_Report:
                    Report();
                    break;
                default:
                    break;
            }
        }

        public override void ResetEventSO()
        {
            SetNextPhase(SubEventPhase.Phase_None);
            //SubEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<SubEventUI>().GetEventUIElement();
        }

        public void SetNextPhase(SubEventPhase Phase)
        {
            subEventPhase = Phase;
        }

        public virtual void Report()
        {
            // ‰¼
            if (Input.GetMouseButtonDown(0))
            {
                ResetEventSO();
                RouteManager.instance.MovePath();
            }
        }
    }
}