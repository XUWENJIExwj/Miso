using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using System.Collections;

namespace EventScriptableObject
{
    public enum SubEventPhase
    {
        Phase_ReportPre,
        Phase_Report,
        Phase_EndingPre,
        Phase_Ending,
        Phase_None,
    }

    [CreateAssetMenu(fileName = "SubEvent_", menuName = "SubEvent")]
    public class SubEventSO : EventSO
    {
        [TextArea(5, 20)] public string eventSummary;
        [TextArea(5, 20)] public string eventReport;
        public float[] bonusRatio = new float[] { 1.0f, 1.2f }; // ëäê´ÅFïÅí ÅAó«Ç¢
        [SerializeField] private SubEventPhase subEventPhase = SubEventPhase.Phase_None;
        public float frameFadeTime = 0.8f;
        public bool autoPlay = false;
        private Tweener tweener = null;

        protected override void Init()
        {
            InitEvent(EventSOType.SubEvent);
        }

        public override void EventStart()
        {
            SubEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<SubEventUI>().GetEventUIElement();
            ui.Title.text = eventTitle;
            ui.Summary.text = eventSummary;

            tweener = ui.TitleFrame.DOFade(1.0f, frameFadeTime).OnUpdate(() =>
            {
                ui.Title.color = HelperFunction.ChangeAlpha(ui.Title.color, ui.TitleFrame.color.a);
                ui.SummaryFrame.color = HelperFunction.ChangeAlpha(ui.SummaryFrame.color, ui.TitleFrame.color.a);
                ui.Summary.color = HelperFunction.ChangeAlpha(ui.Summary.color, ui.TitleFrame.color.a);
            }).OnComplete(() =>
            {
                ui.Title.color = HelperFunction.ChangeAlpha(ui.Title.color, ui.TitleFrame.color.a);
                ui.SummaryFrame.color = HelperFunction.ChangeAlpha(ui.SummaryFrame.color, ui.TitleFrame.color.a);
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
                    ReportPre();
                    break;
                case SubEventPhase.Phase_Report:
                    Report();
                    break;
                case SubEventPhase.Phase_EndingPre:
                    EndingPre();
                    break;
                case SubEventPhase.Phase_Ending:
                    Ending();
                    break;
                default:
                    break;
            }
        }

        public override void ResetEventSO()
        {
            point = 0;
            autoPlay = false;

            SetNextPhase(SubEventPhase.Phase_None);
        }

        public override void AddResult()
        {
            EventUIManager.instance.AddResult(this);
        }

        public override void CheckBouns()
        {
            if (Player.instance.GetCurrentAMAType() == amaType)
            {
                point = (int)(point * bonusRatio[1]);
            }
        }

        public override void SetPointText()
        {
            SubEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<SubEventUI>().GetEventUIElement();
            ui.Point.text = point.ToString("+#;-#;0");
        }

        public void SetNextPhase(SubEventPhase Phase)
        {
            subEventPhase = Phase;
        }

        public virtual void ReportPre()
        {
            if (Input.GetMouseButtonDown(0))
            {
                tweener.Kill();

                SubEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<SubEventUI>().GetEventUIElement();
                ui.TitleFrame.color = HelperFunction.ChangeAlpha(ui.TitleFrame.color, 1.0f);
                ui.Title.color = HelperFunction.ChangeAlpha(ui.Title.color, 1.0f);
                ui.SummaryFrame.color = HelperFunction.ChangeAlpha(ui.SummaryFrame.color, 1.0f);
                ui.Summary.color = HelperFunction.ChangeAlpha(ui.Summary.color, 1.0f);
                SetNextPhase(SubEventPhase.Phase_Report);
            }
        }

        public virtual void Report()
        {
            SubEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<SubEventUI>().GetEventUIElement();
            ui.Report.text = eventReport;

            SetPoint();

            tweener = ui.ReportFrame.DOFade(1.0f, frameFadeTime).OnUpdate(() =>
            {
                ui.Report.color = HelperFunction.ChangeAlpha(ui.Report.color, ui.ReportFrame.color.a);
                ui.PointText.color = HelperFunction.ChangeAlpha(ui.PointText.color, ui.ReportFrame.color.a);
                ui.Point.color = HelperFunction.ChangeAlpha(ui.Point.color, ui.ReportFrame.color.a);
            }).OnComplete(() =>
            {
                ui.Report.color = HelperFunction.ChangeAlpha(ui.Report.color, ui.ReportFrame.color.a);
                ui.PointText.color = HelperFunction.ChangeAlpha(ui.PointText.color, ui.ReportFrame.color.a);
                ui.Point.color = HelperFunction.ChangeAlpha(ui.Point.color, ui.ReportFrame.color.a);

                StartAutoPlay();
                SetNextPhase(SubEventPhase.Phase_Ending);
            });

            SetNextPhase(SubEventPhase.Phase_EndingPre);
        }

        public virtual void EndingPre()
        {
            if (Input.GetMouseButtonDown(0))
            {
                tweener.Kill();

                SubEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<SubEventUI>().GetEventUIElement();
                ui.ReportFrame.color = HelperFunction.ChangeAlpha(ui.ReportFrame.color, 1.0f);
                ui.Report.color = HelperFunction.ChangeAlpha(ui.Report.color, 1.0f);
                ui.PointText.color = HelperFunction.ChangeAlpha(ui.PointText.color, 1.0f);
                ui.Point.color = HelperFunction.ChangeAlpha(ui.Point.color, 1.0f);

                StartAutoPlay();
                SetNextPhase(SubEventPhase.Phase_Ending);
            }
        }

        public virtual void Ending()
        {
            // âº
            if (Input.GetMouseButtonDown(0) || autoPlay)
            {
                SubEventUI eventUI = EventUIManager.instance.GetCurrentEventUI<SubEventUI>();
                eventUI.StopAllCoroutines();
                AddResult();
                ResetEventSO();
                RouteManager.instance.MovePath();
            }
        }

        public override void StartAutoPlay()
        {
            SubEventUI eventUI = EventUIManager.instance.GetCurrentEventUI<SubEventUI>();
            eventUI.StopAllCoroutines();
            eventUI.StartCoroutine(AutoPlay());
        }

        public IEnumerator AutoPlay(float WaitTime = 1.0f)
        {
            float timeCount = 0.0f;

            while (timeCount < WaitTime)
            {
                yield return new WaitForEndOfFrame();
                timeCount += Time.deltaTime;
            }

            autoPlay = true;
        }
    }
}