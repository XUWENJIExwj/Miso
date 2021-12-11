using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

namespace EventScriptableObject
{
    public enum RandomEventPhase
    {
        Phase_ReportPre,
        Phase_Report,
        Phase_EndingPre,
        Phase_Ending,
        Phase_None,
    }

    [CreateAssetMenu(fileName = "RandomEvent_", menuName = "RandomEvent")]
    public class RandomEventSO : SubEventSO
    {
        [SerializeField] private RandomEventPhase randomEventPhase = RandomEventPhase.Phase_None;
        private Tweener tweener = null;

        protected override void Init()
        {
            InitEvent(EventSOType.RandomEvent);
        }

        public override void EventStart()
        {
            SubEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<RandomEventUI>().GetEventUIElement();
            ui.Title.text = eventTitle;
            //ui.Summary.text = eventSummary;

            tweener = ui.TitleFrame.DOFade(1.0f, frameFadeTime).OnUpdate(() =>
            {
                ui.Title.color = HelperFunction.ChangeAlpha(ui.Title.color, ui.TitleFrame.color.a);
                //ui.SummaryFrame.color = HelperFunction.ChangeAlpha(ui.SummaryFrame.color, ui.TitleFrame.color.a);
                //ui.Summary.color = HelperFunction.ChangeAlpha(ui.Summary.color, ui.TitleFrame.color.a);
            }).OnComplete(() =>
            {
                ui.Title.color = HelperFunction.ChangeAlpha(ui.Title.color, ui.TitleFrame.color.a);
                //ui.SummaryFrame.color = HelperFunction.ChangeAlpha(ui.SummaryFrame.color, ui.TitleFrame.color.a);
                //ui.Summary.color = HelperFunction.ChangeAlpha(ui.Summary.color, ui.TitleFrame.color.a);
                SetNextPhase(RandomEventPhase.Phase_Report);
            });

            SetNextPhase(RandomEventPhase.Phase_ReportPre);
        }

        public override void EventPlay()
        {
            switch (randomEventPhase)
            {
                case RandomEventPhase.Phase_ReportPre:
                    ReportPre();
                    break;
                case RandomEventPhase.Phase_Report:
                    Report();
                    break;
                case RandomEventPhase.Phase_EndingPre:
                    EndingPre();
                    break;
                case RandomEventPhase.Phase_Ending:
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

            SetNextPhase(RandomEventPhase.Phase_None);
        }

        public override void AddResult()
        {
            EventUIManager.instance.AddResult(this);
        }

        public override void CheckBouns()
        {

        }

        public override void SetPointText()
        {
            SubEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<RandomEventUI>().GetEventUIElement();
            ui.Point.text = point.ToString("+#;-#;0");
        }

        public void SetNextPhase(RandomEventPhase Phase)
        {
            randomEventPhase = Phase;
        }

        public override void ReportPre()
        {
            if (Input.GetMouseButtonDown(0))
            {
                tweener.Kill();

                SubEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<RandomEventUI>().GetEventUIElement();
                ui.TitleFrame.color = HelperFunction.ChangeAlpha(ui.TitleFrame.color, 1.0f);
                ui.Title.color = HelperFunction.ChangeAlpha(ui.Title.color, 1.0f);
                //ui.SummaryFrame.color = HelperFunction.ChangeAlpha(ui.SummaryFrame.color, 1.0f);
                //ui.Summary.color = HelperFunction.ChangeAlpha(ui.Summary.color, 1.0f);
                SetNextPhase(RandomEventPhase.Phase_Report);
            }
        }

        public override void Report()
        {
            SubEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<RandomEventUI>().GetEventUIElement();
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
                SetNextPhase(RandomEventPhase.Phase_Ending);
            });

            SetNextPhase(RandomEventPhase.Phase_EndingPre);
        }

        public override void EndingPre()
        {
            if (Input.GetMouseButtonDown(0))
            {
                tweener.Kill();

                SubEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<RandomEventUI>().GetEventUIElement();
                ui.ReportFrame.color = HelperFunction.ChangeAlpha(ui.ReportFrame.color, 1.0f);
                ui.Report.color = HelperFunction.ChangeAlpha(ui.Report.color, 1.0f);
                ui.PointText.color = HelperFunction.ChangeAlpha(ui.PointText.color, 1.0f);
                ui.Point.color = HelperFunction.ChangeAlpha(ui.Point.color, 1.0f);

                StartAutoPlay();
                SetNextPhase(RandomEventPhase.Phase_Ending);
            }
        }

        public override void Ending()
        {
            // ‰¼
            if (Input.GetMouseButtonDown(0) || autoPlay)
            {
                RandomEventUI eventUI = EventUIManager.instance.GetCurrentEventUI<RandomEventUI>();
                eventUI.StopAllCoroutines();

                AddResult();
                ResetEventSO();
                Player.instance.SetRandomEventCompleted(id);
                RouteManager.instance.MovePath();
            }
        }

        public override void StartAutoPlay()
        {
            RandomEventUI eventUI = EventUIManager.instance.GetCurrentEventUI<RandomEventUI>();
            eventUI.StopAllCoroutines();
            eventUI.StartCoroutine(AutoPlay());
        }
    }
}
