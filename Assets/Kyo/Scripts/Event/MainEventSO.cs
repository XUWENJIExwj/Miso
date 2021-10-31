using UnityEngine;
using System;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Text.RegularExpressions;

namespace EventScriptableObject
{
    public enum CharacterTypes
    {
        Player,
        AMA_Higashi,
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
        public CharacterTypes type;
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
        public CharacterText[] reports;
        public MainEventOption[] optionFirst;
        public MainEventOptionNext[] optionNext;
        public MainEventOptionEx[] optionSecond;
        public MainEventPhase mainEventPhase = MainEventPhase.Phase_None;
        public MainEventProgress progress;
        public float frameFadeTime = 0.8f;
        public float printInterval = 0.1f;
        public bool onPrint = false;
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
            ui.Character.sprite = reports[progress.characterIndex].sprite;

            tweener = ui.TitleFrame.DOFade(1.0f, frameFadeTime).OnUpdate(() =>
            {
                ui.Title.color = HelperFunction.ChangeAlpha(ui.Title.color, ui.TitleFrame.color.a);
                ui.Summary.color = HelperFunction.ChangeAlpha(ui.Summary.color, ui.TitleFrame.color.a);
            }).OnComplete(() =>
            {
                ui.Title.color = HelperFunction.ChangeAlpha(ui.Title.color, ui.TitleFrame.color.a);
                ui.Summary.color = HelperFunction.ChangeAlpha(ui.Summary.color, ui.TitleFrame.color.a);

                tweener = ui.Character.DOFade(1.0f, frameFadeTime).OnComplete(() =>
                {
                    tweener = ui.TalkFrame.DOFade(1.0f, frameFadeTime).OnUpdate(() =>
                    {
                        ui.Name.color = HelperFunction.ChangeAlpha(ui.Name.color, ui.TalkFrame.color.a);
                        ui.Talk.color = HelperFunction.ChangeAlpha(ui.Talk.color, ui.TalkFrame.color.a);
                    }).OnComplete(() =>
                    {
                        ui.Name.color = HelperFunction.ChangeAlpha(ui.Name.color, ui.TalkFrame.color.a);
                        ui.Talk.color = HelperFunction.ChangeAlpha(ui.Talk.color, ui.TalkFrame.color.a);
                        ui.Name.text = reports[progress.characterIndex].name;

                        SetNextPhase(MainEventPhase.Phase_Report);
                        StartCoroutinePrintCharacterText(reports[progress.characterIndex], mainEventPhase);
                    });
                });
            });
            
            SetNextPhase(MainEventPhase.Phase_ReportPre);
        }

        public override void EventPlay()
        {
            switch(mainEventPhase)
            {
                case MainEventPhase.Phase_ReportPre:
                    ReportPre();
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
            mainEventPhase = Phase;
        }

        public void ReportPre()
        {
            if (Input.GetMouseButtonDown(0) && tweener.IsActive())
            {
                tweener.Kill();

                MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
                ui.TitleFrame.color = HelperFunction.ChangeAlpha(ui.TitleFrame.color, 1.0f);
                ui.Title.color = HelperFunction.ChangeAlpha(ui.Title.color, 1.0f);
                ui.Summary.color = HelperFunction.ChangeAlpha(ui.Summary.color, 1.0f);
                ChangeCharacterImage(ref ui.Character, reports[progress.characterIndex].sprite);
                ui.TalkFrame.color = HelperFunction.ChangeAlpha(ui.TalkFrame.color, 1.0f);
                ui.Name.color = HelperFunction.ChangeAlpha(ui.Name.color, 1.0f);
                ui.Talk.color = HelperFunction.ChangeAlpha(ui.Talk.color, 1.0f);
                ui.Name.text = reports[progress.characterIndex].name;

                SetNextPhase(MainEventPhase.Phase_Report);
                StartCoroutinePrintCharacterText(reports[progress.characterIndex], mainEventPhase);
                StopAllCoroutinePrintCharacterText(reports[progress.characterIndex], mainEventPhase);
            }
        }

        public override void Report()
        {
            UpdateCharacterText(reports);
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

            UpdateCharacterText(optionFirst[progress.optionRouteFirst].character);
        }

        public void OptionNextPre()
        {
            progress.characterIndex = 0;
            progress.textIndex = 0;

            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ChangeCharacterImage(ref ui.Character, optionNext[progress.optionRouteFirst].character[progress.characterIndex].sprite);
            ui.Name.text = optionNext[progress.optionRouteFirst].character[progress.characterIndex].name;
            
            SetNextPhase(MainEventPhase.Phase_OptionNext);
            StartCoroutinePrintCharacterText(optionNext[progress.optionRouteFirst].character[progress.characterIndex], mainEventPhase);
        }

        public void OptionNext()
        {
            if (progress.optionRouteFirst < 0)
            {
                return;
            }

            UpdateCharacterText(optionNext[progress.optionRouteFirst].character);
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

            UpdateCharacterText(optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].character);
        }

        public void EndingPre()
        {
            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ui.Summary.text = optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].ending[0].type.ToString();
            ChangeCharacterImage(ref ui.Character, null);
            ui.Name.text = "";
            
            SetNextPhase(MainEventPhase.Phase_Ending);
            StartCoroutinePrintText(optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].ending[0].endingText, mainEventPhase);
        }

        public void Ending()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (onPrint)
                {
                    StopAllCoroutinePrintText(optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].ending[0].endingText, mainEventPhase);
                }
                else
                {
                    ResetEventSO();
                    RouteManager.instance.MovePath();
                }
            }
        }

        public void UpdateCharacterText(CharacterText[] Characters)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (onPrint)
                {
                    StopAllCoroutinePrintCharacterText(Characters[progress.characterIndex], mainEventPhase);
                }
                else
                {
                    MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();

                    // 最後のCharacterではない場合
                    if (progress.characterIndex < Characters.Length - 1)
                    {
                        // 現在のCharacterのセリフが最後ではない場合
                        if (progress.textIndex < Characters[progress.characterIndex].texts.Length)
                        {
                            StartCoroutinePrintCharacterText(Characters[progress.characterIndex], mainEventPhase);
                        }
                        // 現在のCharacterのセリフが最後の場合
                        else
                        {
                            ++progress.characterIndex;
                            progress.textIndex = 0;

                            ChangeCharacterImage(ref ui.Character, Characters[progress.characterIndex].sprite);
                            ui.Name.text = Characters[progress.characterIndex].name;

                            StartCoroutinePrintCharacterText(Characters[progress.characterIndex], mainEventPhase);
                        }
                    }
                    // 最後のCharacterの場合
                    else
                    {
                        // 最後のセリフではない場合
                        if (progress.textIndex < Characters[progress.characterIndex].texts.Length)
                        {
                            StartCoroutinePrintCharacterText(Characters[progress.characterIndex], mainEventPhase);
                        }
                        // 最後のセリフの場合
                        else
                        {
                            NextPhase();
                        }
                    }
                }
            }
        }

        public void NextPhase()
        {
            switch(mainEventPhase)
            {
                case MainEventPhase.Phase_Report:
                    OptionFirstPre();
                    break;
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

            StartCoroutinePrintCharacterText(optionFirst[progress.optionRouteFirst].character[progress.characterIndex], mainEventPhase);
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

            StartCoroutinePrintCharacterText(optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].character[progress.characterIndex], mainEventPhase);
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

        public void StartCoroutinePrintCharacterText(CharacterText Character, MainEventPhase Phase)
        {
            EventUIManager.instance.StartCoroutine(StartPrintCharacterText(Character, Phase));
        }

        public void StartCoroutinePrintText(string Text, MainEventPhase Phase)
        {
            EventUIManager.instance.StartCoroutine(StartPrintText(Text, Phase));
        }

        public void StopAllCoroutinePrintCharacterText(CharacterText Character, MainEventPhase Phase)
        {
            EventUIManager.instance.StopAllCoroutines();
            StopPrintCharacterText(Character, Phase);
        }

        public void StopAllCoroutinePrintText(string Text, MainEventPhase Phase)
        {
            EventUIManager.instance.StopAllCoroutines();
            StopPrintText(Text, Phase);
        }

        public IEnumerator StartPrintCharacterText(CharacterText Character, MainEventPhase Phase)
        {
            onPrint = true;

            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ui.Talk.text = "";

            for (int i = 0; i < Character.texts[progress.textIndex].Length; ++i)
            {
                i = CheckNewExpression(Character, i);

                ui.Talk.text += Character.texts[progress.textIndex][i];
                yield return new WaitForSeconds(printInterval);
            }

            onPrint = false;
            ++progress.textIndex;
            SetNextPhase(Phase);
        }

        public IEnumerator StartPrintText(string Text, MainEventPhase Phase)
        {
            onPrint = true;

            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ui.Talk.text = "";

            foreach (var item in Text)
            {
                ui.Talk.text += item;
                yield return new WaitForSeconds(printInterval);
            }

            onPrint = false;
            ++progress.textIndex;
            SetNextPhase(Phase);
        }

        public void StopPrintCharacterText(CharacterText Character, MainEventPhase Phase)
        {
            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ui.Talk.text = "";

            for (int i = 0; i < Character.texts[progress.textIndex].Length; ++i)
            {
                i = CheckNewExpression(Character, i);

                ui.Talk.text += Character.texts[progress.textIndex][i];
            }

            onPrint = false;
            ++progress.textIndex;

            SetNextPhase(Phase);
        }

        public void StopPrintText(string Text, MainEventPhase Phase)
        {
            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ui.Talk.text = Text;

            onPrint = false;
            ++progress.textIndex;

            SetNextPhase(Phase);
        }

        public int CheckNewExpression(CharacterText Character, int TextIndex)
        {
            string expressionType = "";
            string symbol = "（）";
            int textIndex = TextIndex;

            if (Character.texts[progress.textIndex][TextIndex] == symbol[0])
            {
                expressionType += Character.texts[progress.textIndex][TextIndex];
                while (Character.texts[progress.textIndex][TextIndex] != symbol[1])
                {
                    ++TextIndex;
                    expressionType += Character.texts[progress.textIndex][TextIndex];
                }

                Sprite expression = IllustrationDictionary.instance.GetTargetIllustration(Character.type, expressionType);
                if (expression)
                {
                    MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
                    ui.Character.sprite = expression;
                    expressionType = "";
                    ++TextIndex;
                    textIndex = TextIndex;
                }
            }

            return textIndex;
        }
    }
}