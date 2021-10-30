using UnityEngine;
using System;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

namespace EventScriptableObject
{
    public enum CharacterType
    {
        Player,
        AMA,
        NPC,
        Aside,
        None,
    }

    public enum CompletedType
    {
        Failed,
        Succeeded,
        Special,
    }

    [Serializable]
    public struct CharacterText
    {
        public CharacterType type;
        public Sprite sprite;
        public string name;
        [TextArea(5, 20)] public string[] texts;
    }

    [Serializable]
    public struct EndingObject
    {
        public CompletedType type;
        [TextArea(5, 20)] public string endingText;
    }

    [Serializable]
    public struct MainEventOption
    {
        [TextArea(5, 20)] public string optionText;
        public CharacterText[] character;
        public EndingObject[] ending;
    }

    [Serializable]
    public struct MainEventOptionNext
    {
        public CharacterText[] character;
    }

    [Serializable]
    public struct MainEventOptionEx
    {
        public MainEventOption[] options;
    }

    [Serializable]
    public struct MainEventProgress
    {
        public int optionRouteFirst;
        public int optionRouteSecond;
        public int characterIndex;
        public int textIndex;
    }

    public enum MainEventPhase
    {
        Phase_ReportPre,
        Phase_Report,
        Phase_OptionFirst,
        Phase_OptionNext,
        Phase_OptionSecond,
        Phase_Ending,
        Phase_None,
    }

    [CreateAssetMenu(fileName = "MainEvent_", menuName = "MainEvent")]
    public class MainEventSO : SubEventSO
    {
        [Header("MainEvent")]
        public CharacterText report;
        public MainEventOption[] optionFirst;
        public MainEventOptionNext[] optionNext;
        public MainEventOptionEx[] optionSecond;
        public MainEventPhase phase = MainEventPhase.Phase_None;
        public MainEventProgress progress;
        private Tweener tweener = null;
        private UnityAction[] onClickFirst = null;
        private UnityAction[] onClickSecond = null;

        protected override void Init()
        {
            InitEvent(EventButtonType.MainEvent);
        }

        public override void EventStart()
        {
            // 動的にOnClickのイベントを変更できるように
            // 必要なイベントを予め設定しておく
            if (onClickFirst == null)
            {
                onClickFirst = new UnityAction[2];
                onClickFirst[0] = delegate { OnClickFirst(0); };
                onClickFirst[1] = delegate { OnClickFirst(1); };
            }

            if (onClickSecond == null)
            {
                onClickSecond = new UnityAction[2];
                onClickSecond[0] = delegate { OnClickSecond(0); };
                onClickSecond[1] = delegate { OnClickSecond(1); };
            }

            ResetProgress();
            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ui.Title.text = eventTitle;
            ui.Summary.text = eventSummary;
            ui.Character.sprite = report.sprite;
            ui.Name.text = report.name;
            ui.Talk.text = report.texts[progress.textIndex];
            ++progress.textIndex;

            // 仮
            float time = 0.8f;
            tweener = ui.TitleFrame.DOFade(1.0f, time).OnUpdate(() =>
            {
                ui.Title.color = HelperFunction.ChangeAlpha(ui.Title.color, ui.TitleFrame.color.a);
                ui.Summary.color = HelperFunction.ChangeAlpha(ui.Summary.color, ui.TitleFrame.color.a);
            }).OnComplete(() =>
            {
                ui.Title.color = HelperFunction.ChangeAlpha(ui.Title.color, ui.TitleFrame.color.a);
                ui.Summary.color = HelperFunction.ChangeAlpha(ui.Summary.color, ui.TitleFrame.color.a);

                ui.Character.DOFade(1.0f, time).OnComplete(() =>
                {
                    ui.TalkFrame.DOFade(1.0f, time).OnUpdate(() =>
                    {
                        ui.Name.color = HelperFunction.ChangeAlpha(ui.Name.color, ui.TalkFrame.color.a);
                        ui.Talk.color = HelperFunction.ChangeAlpha(ui.Talk.color, ui.TalkFrame.color.a);
                    }).OnComplete(() =>
                    {
                        ui.Name.color = HelperFunction.ChangeAlpha(ui.Name.color, ui.TalkFrame.color.a);
                        ui.Talk.color = HelperFunction.ChangeAlpha(ui.Talk.color, ui.TalkFrame.color.a);

                        SetNextPhase(MainEventPhase.Phase_Report);
                    });
                });
            });

            SetNextPhase(MainEventPhase.Phase_ReportPre);
        }

        public override void EventPlay()
        {
            switch(phase)
            {
                case MainEventPhase.Phase_ReportPre:
                    break;
                case MainEventPhase.Phase_Report:
                    Report();
                    break;
                case MainEventPhase.Phase_OptionFirst:
                    OptionFirst();
                    break;
                case MainEventPhase.Phase_OptionNext:
                    OptionNext();
                    break;
                case MainEventPhase.Phase_OptionSecond:
                    OptionSecond();
                    break;
                case MainEventPhase.Phase_Ending:
                    Ending();
                    break;
                default:
                    break;
            }
        }

        public override void ResetEventSO()
        {
            SetNextPhase(MainEventPhase.Phase_None);
            ResetProgress();

            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ui.Options[0].onClick.RemoveAllListeners();
            ui.Options[1].onClick.RemoveAllListeners();
        }

        public void SetNextPhase(MainEventPhase Phase)
        {
            phase = Phase;
        }

        public void Report()
        {
            if (Input.GetMouseButtonDown(0))
            {
                // 最後のセリフではない場合
                if (progress.textIndex < report.texts.Length)
                {
                    MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();

                    ChangeCharacterImage(ref ui.Character, report.sprite);

                    ui.Name.text = report.name;
                    ui.Talk.text = report.texts[progress.textIndex];

                    ++progress.textIndex;
                }
                // 最後のセリフの場合
                else
                {
                    OptionFirstPre();
                }
            }
        }

        public void OptionFirstPre()
        {
            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ui.OptionParent.SetActive(true);
            ui.Options[0].onClick.AddListener(onClickFirst[0]);
            ui.Options[0].gameObject.GetComponentInChildren<TMP_Text>().text = optionFirst[0].optionText;
            ui.Options[1].onClick.AddListener(onClickFirst[1]);
            ui.Options[1].gameObject.GetComponentInChildren<TMP_Text>().text = optionFirst[1].optionText;

            SetNextPhase(MainEventPhase.Phase_OptionFirst);
        }

        public void OptionFirst()
        {
            if(progress.optionRouteFirst < 0)
            {
                return;
            }

            UpdateText(optionFirst[progress.optionRouteFirst].character);
        }

        public void OptionNextPre()
        {
            progress.characterIndex = 0;
            progress.textIndex = 0;

            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();

            ChangeCharacterImage(ref ui.Character, optionNext[progress.optionRouteFirst].character[progress.characterIndex].sprite);

            ui.Name.text = optionNext[progress.optionRouteFirst].character[progress.characterIndex].name;
            ui.Talk.text = optionNext[progress.optionRouteFirst].character[progress.characterIndex].texts[progress.textIndex];

            ++progress.textIndex;

            SetNextPhase(MainEventPhase.Phase_OptionNext);
        }

        public void OptionNext()
        {
            if (progress.optionRouteFirst < 0)
            {
                return;
            }

            UpdateText(optionNext[progress.optionRouteFirst].character);
        }

        public void OptionSecondPre()
        {
            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ui.OptionParent.SetActive(true);
            ui.Options[0].onClick.RemoveListener(onClickFirst[0]);
            ui.Options[0].onClick.AddListener(onClickSecond[0]);
            ui.Options[0].gameObject.GetComponentInChildren<TMP_Text>().text = optionSecond[progress.optionRouteFirst].options[0].optionText;
            ui.Options[1].onClick.RemoveListener(onClickFirst[1]);
            ui.Options[1].onClick.AddListener(onClickSecond[1]);
            ui.Options[1].gameObject.GetComponentInChildren<TMP_Text>().text = optionSecond[progress.optionRouteFirst].options[1].optionText;

            SetNextPhase(MainEventPhase.Phase_OptionSecond);
        }

        public void OptionSecond()
        {
            if (progress.optionRouteSecond < 0)
            {
                return;
            }

            UpdateText(optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].character);
        }

        public void EndingPre()
        {
            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ui.Summary.text = optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].ending[0].type.ToString();
            ui.Talk.text = optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].ending[0].endingText;
            SetNextPhase(MainEventPhase.Phase_Ending);
        }

        public void Ending()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ResetEventSO();
                RouteManager.instance.MovePath();
            }
        }

        public void UpdateText(CharacterText[] Texts)
        {
            if (Input.GetMouseButtonDown(0))
            {
                MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();

                // 最後のCharacterではない場合
                if (progress.characterIndex < Texts.Length - 1)
                {
                    // 現在のCharacterのセリフが最後ではない場合
                    if (progress.textIndex < Texts[progress.characterIndex].texts.Length)
                    {

                        ui.Talk.text = Texts[progress.characterIndex].texts[progress.textIndex];
                        ++progress.textIndex;
                    }
                    // 現在のCharacterのセリフが最後の場合
                    else
                    {
                        ++progress.characterIndex;
                        progress.textIndex = 0;

                        ChangeCharacterImage(ref ui.Character, Texts[progress.characterIndex].sprite);

                        ui.Name.text = Texts[progress.characterIndex].name;
                        ui.Talk.text = Texts[progress.characterIndex].texts[progress.textIndex];

                        ++progress.textIndex;
                    }
                }
                // 最後のCharacterの場合
                else
                {
                    // 最後のセリフではない場合
                    if (progress.textIndex < Texts[progress.characterIndex].texts.Length)
                    {
                        ui.Talk.text = Texts[progress.characterIndex].texts[progress.textIndex];
                        ++progress.textIndex;
                    }
                    // 最後のセリフの場合
                    else
                    {
                        NextPhase();
                    }
                }
            }
        }

        public void NextPhase()
        {
            switch(phase)
            {
                case MainEventPhase.Phase_OptionFirst:
                    OptionNextPre();
                    break;
                case MainEventPhase.Phase_OptionNext:
                    OptionSecondPre();
                    break;
                case MainEventPhase.Phase_OptionSecond:
                    EndingPre();
                    break;
                default:
                    break;
            }
        }

        public void OnClickFirst(int OptionRoute)
        {
            progress.optionRouteFirst = OptionRoute;
            progress.characterIndex = 0;
            progress.textIndex = 0;

            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ui.OptionParent.SetActive(false);

            ChangeCharacterImage(ref ui.Character, optionFirst[progress.optionRouteFirst].character[progress.characterIndex].sprite);

            ui.Name.text = optionFirst[progress.optionRouteFirst].character[progress.characterIndex].name;
            ui.Talk.text = optionFirst[progress.optionRouteFirst].character[progress.characterIndex].texts[progress.textIndex];

            ++progress.textIndex;
        }

        public void OnClickSecond(int OptionRoute)
        {
            progress.optionRouteSecond = OptionRoute;
            progress.characterIndex = 0;
            progress.textIndex = 0;

            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ui.OptionParent.SetActive(false);

            ChangeCharacterImage(ref ui.Character, optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].character[progress.characterIndex].sprite);

            ui.Name.text = optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].character[progress.characterIndex].name;
            ui.Talk.text = optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].character[progress.characterIndex].texts[progress.textIndex];

            ++progress.textIndex;
        }

        public void ResetProgress(int OptionRouteFirst = -1, int OptionRouteSecond = -1, int CharacterIndex = 0, int TextIndex = 0)
        {
            progress.optionRouteFirst = OptionRouteFirst;
            progress.optionRouteSecond = OptionRouteSecond;
            progress.characterIndex = CharacterIndex;
            progress.textIndex = TextIndex;
        }

        public void ChangeCharacterImage(ref Image CharacterImage, Sprite CharacterSprite)
        {
            if (CharacterSprite)
            {
                CharacterImage.color = HelperFunction.ChangeAlpha(CharacterImage.color, 1.0f);
            }
            else
            {
                CharacterImage.color = HelperFunction.ChangeAlpha(CharacterImage.color, 0.0f);
            }

            CharacterImage.sprite = CharacterSprite;
        }
    }
}