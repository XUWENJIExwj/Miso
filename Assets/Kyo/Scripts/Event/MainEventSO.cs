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
        public PointRange point;
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
        public float printInterval = 0.1f;
        public bool onPrint = false;
        private Tweener tweener = null;
        private UnityAction[] onClickFirst = null;
        private UnityAction[] onClickSecond = null;

        protected override void Init()
        {
            InitEvent(EventSOType.MainEvent);
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

            // MainEventの各要素の出現のアニメーション
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
            point = 0;

            SetNextPhase(MainEventPhase.Phase_None);
            ResetProgress();

            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ui.Options[0].onClick.RemoveAllListeners();
            ui.Options[1].onClick.RemoveAllListeners();
        }

        public override void AddResult()
        {
            EventUIManager.instance.AddResult(this);
        }

        public override void ComputePoint()
        {
            pointRange.min = optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].ending[0].point.min;
            pointRange.max = optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].ending[0].point.max;
            base.ComputePoint();
        }

        public override void SetPointText()
        {
            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ui.Point.text = "Point: " + point.ToString("+#;-#;0");
        }

        public void SetNextPhase(MainEventPhase Phase)
        {
            mainEventPhase = Phase;
        }

        // Phase_Reportの前処理
        public override void ReportPre()
        {
            // MainEventの各要素の出現のアニメーションを止めて、Phase_Reportへ突入
            if (Input.GetMouseButtonDown(0))
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

        // Phase_Report
        public override void Report()
        {
            UpdateCharacterText(reports);
        }

        // Phase_OptionFirstの前処理
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

        // Phase_OptionFirst
        public void OptionFirst()
        {
            if(progress.optionRouteFirst < 0)
            {
                return;
            }

            UpdateCharacterText(optionFirst[progress.optionRouteFirst].character);
        }

        // Phase_OptionNextの前処理
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

        // Phase_OptionNext
        public void OptionNext()
        {
            if (progress.optionRouteFirst < 0)
            {
                return;
            }

            UpdateCharacterText(optionNext[progress.optionRouteFirst].character);
        }

        // Phase_OptionSecondの前処理
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

        // Phase_OptionSecond
        public void OptionSecond()
        {
            if (progress.optionRouteSecond < 0)
            {
                return;
            }

            UpdateCharacterText(optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].character);
        }

        // Phase_Endingの前処理
        public override void EndingPre()
        {
            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ui.Summary.text = optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].ending[0].type.ToString();
            ChangeCharacterImage(ref ui.Character, null);
            ui.Name.text = "";

            SetPoint();
            SetNextPhase(MainEventPhase.Phase_Ending);
            StartCoroutinePrintEndingText(optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].ending[0].endingText, mainEventPhase);
        }

        // Phase_Ending
        public override void Ending()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (onPrint)
                {
                    StopAllCoroutinePrintEndingText(optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].ending[0].endingText, mainEventPhase);
                }
                else
                {
                    AddResult();
                    ResetEventSO();
                    RouteManager.instance.MovePath();
                }
            }
        }

        // MainEventの会話の進行処理
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

        // 前処理突入
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

        // 一番目の選択処理
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

        // 二番目の選択処理
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

        // MainEventの会話の進捗
        public void ResetProgress(int OptionRouteFirst = -1, int OptionRouteSecond = -1, int CharacterIndex = 0, int TextIndex = 0)
        {
            progress.optionRouteFirst = OptionRouteFirst;
            progress.optionRouteSecond = OptionRouteSecond;
            progress.characterIndex = CharacterIndex;
            progress.textIndex = TextIndex;
        }

        // イラストを切り替える処理
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

        // CharacterTextの文字送りアニメーションのCoroutineをスタート
        public void StartCoroutinePrintCharacterText(CharacterText Character, MainEventPhase Phase)
        {
            EventUIManager.instance.StartCoroutine(StartPrintCharacterText(Character, Phase));
        }

        // Textの文字送りアニメーションのCoroutineをスタート
        public void StartCoroutinePrintEndingText(string Text, MainEventPhase Phase)
        {
            EventUIManager.instance.StartCoroutine(StartPrintEndingText(Text, Phase));
        }

        // CharacterTextの文字送りアニメーションのCoroutineをストップ
        public void StopAllCoroutinePrintCharacterText(CharacterText Character, MainEventPhase Phase)
        {
            EventUIManager.instance.StopAllCoroutines();
            StopPrintCharacterText(Character, Phase);
        }

        // Textの文字送りアニメーションのCoroutineをストップ
        public void StopAllCoroutinePrintEndingText(string Text, MainEventPhase Phase)
        {
            EventUIManager.instance.StopAllCoroutines();
            StopPrintEndingText(Text, Phase);
        }

        // CharacterTextの文字送りアニメーションのCoroutine
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

        // Textの文字送りアニメーションのCoroutine
        public IEnumerator StartPrintEndingText(string Text, MainEventPhase Phase)
        {
            onPrint = true;

            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ui.Talk.text = "";

            foreach (var item in Text)
            {
                ui.Talk.text += item;
                yield return new WaitForSeconds(printInterval);
            }

            tweener = ui.Point.DOFade(1.0f, frameFadeTime);
            tweener.OnComplete(() => 
            {
                onPrint = false;
                ++progress.textIndex;
                SetNextPhase(Phase);
            });
        }

        // CharacterTextの文字送りアニメーションのCoroutineの中止処理
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

        // Textの文字送りアニメーションのCoroutineの中止処理
        public void StopPrintEndingText(string Text, MainEventPhase Phase)
        {
            if (tweener.IsActive())
            {
                tweener.Kill();
            }

            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ui.Talk.text = Text;
            ui.Point.color = HelperFunction.ChangeAlpha(ui.Point.color, 1.0f);

            onPrint = false;
            ++progress.textIndex;

            SetNextPhase(Phase);
        }

        // MainEventの会話中のCharacterの表情文字列の検索処理
        public int CheckNewExpression(CharacterText Character, int CharIndex)
        {
            string expressionType = "";
            string symbol = "（）";
            int charIndex = CharIndex;

            if (Character.texts[progress.textIndex][CharIndex] == symbol[0])
            {
                expressionType += Character.texts[progress.textIndex][CharIndex];
                while (Character.texts[progress.textIndex][CharIndex] != symbol[1])
                {
                    ++CharIndex;
                    expressionType += Character.texts[progress.textIndex][CharIndex];
                }

                Sprite expression = IllustrationDictionary.instance.GetTargetIllustration(Character.type, expressionType);
                if (expression)
                {
                    MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
                    ui.Character.sprite = expression;
                    expressionType = "";
                    ++CharIndex;
                    charIndex = CharIndex;
                }
            }

            return charIndex;
        }

        public string GetEnding()
        {
            return optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].ending[0].endingText;
        }
    }
}