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
    public enum MainEventCharacterTypes
    {
        Player,
        NPC,
        Aside,
        AMA,
        None,
    }

    public enum JudgementType
    {
        Failed,
        Succeeded,
        Special,
        None,
    }

    [Serializable]
    public struct CharacterText
    {
        public MainEventCharacterTypes type;
        public ExpressionTypes expression;
        public string name;
        [TextArea(5, 20)] public string[] texts;
    }

    [Serializable]
    public struct EndingObject
    {
        public JudgementType type;
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
        public PointRange[] endingPoints;
        public CharacterText[] reports;
        public MainEventOption[] optionFirst;
        public MainEventOptionNext[] optionNext;
        public MainEventOptionEx[] optionSecond;
        [SerializeField] private MainEventPhase mainEventPhase = MainEventPhase.Phase_None;
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

        public override void MakePointRange()
        {
            pointRange.min = int.MaxValue;
            pointRange.max = int.MinValue;
            for (int i = 0; i < endingPoints.Length; ++i)
            {
                pointRange.min = Mathf.Min(pointRange.min, endingPoints[i].min);
                pointRange.max = Mathf.Max(pointRange.max, endingPoints[i].max);
            }
        }

        public override void EventStart()
        {
            // ���I��OnClick�̃C�x���g��ύX�ł���悤��
            // �K�v�ȃC�x���g��\�ߐݒ肵�Ă���
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

            // MainEvent�̊e�v�f�̏o���̃A�j���[�V����
            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ui.Title.text = eventTitle;
            ui.Summary.text = eventSummary;
            ChangeCharacterSpriteFromDictionary(ref ui.Character, reports[progress.characterIndex].type, reports[progress.characterIndex].expression);
            ui.Character.color = HelperFunction.ChangeAlpha(ui.Character.color, 0.0f);

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
                        ui.NameFrame.color = HelperFunction.ChangeAlpha(ui.NameFrame.color, ui.TalkFrame.color.a);
                        ui.Name.color = HelperFunction.ChangeAlpha(ui.Name.color, ui.TalkFrame.color.a);
                        ui.Talk.color = HelperFunction.ChangeAlpha(ui.Talk.color, ui.TalkFrame.color.a);
                    }).OnComplete(() =>
                    {
                        ui.NameFrame.color = HelperFunction.ChangeAlpha(ui.NameFrame.color, ui.TalkFrame.color.a);
                        ui.Name.color = HelperFunction.ChangeAlpha(ui.Name.color, ui.TalkFrame.color.a);
                        ui.Talk.color = HelperFunction.ChangeAlpha(ui.Talk.color, ui.TalkFrame.color.a);
                        ui.Name.text = ChangeCharacterName(reports[progress.characterIndex]);

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

        public override void ComputePoint(JudgementType Judgement)
        {
            pointRange.min = endingPoints[(int)Judgement].min;
            pointRange.max = endingPoints[(int)Judgement].max;
            base.ComputePoint(Judgement);
        }

        public override void SetPointText()
        {
            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ui.Point.text = point.ToString("+#;-#;0");
        }

        public void SetNextPhase(MainEventPhase Phase)
        {
            mainEventPhase = Phase;
        }

        // Phase_Report�̑O����
        public override void ReportPre()
        {
            // MainEvent�̊e�v�f�̏o���̃A�j���[�V�������~�߂āAPhase_Report�֓˓�
            if (Input.GetMouseButtonDown(0))
            {
                tweener.Kill();

                MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
                ui.TitleFrame.color = HelperFunction.ChangeAlpha(ui.TitleFrame.color, 1.0f);
                ui.Title.color = HelperFunction.ChangeAlpha(ui.Title.color, 1.0f);
                ui.Summary.color = HelperFunction.ChangeAlpha(ui.Summary.color, 1.0f);
                ChangeCharacterSpriteFromDictionary(ref ui.Character, reports[progress.characterIndex].type, reports[progress.characterIndex].expression);
                ui.TalkFrame.color = HelperFunction.ChangeAlpha(ui.TalkFrame.color, 1.0f);
                ui.NameFrame.color = HelperFunction.ChangeAlpha(ui.NameFrame.color, 1.0f);
                ui.Name.color = HelperFunction.ChangeAlpha(ui.Name.color, 1.0f);
                ui.Talk.color = HelperFunction.ChangeAlpha(ui.Talk.color, 1.0f);
                ui.Name.text = ChangeCharacterName(reports[progress.characterIndex]);

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

        // Phase_OptionFirst�̑O����
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

        // Phase_OptionNext�̑O����
        public void OptionNextPre()
        {
            progress.characterIndex = 0;
            progress.textIndex = 0;

            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ChangeCharacterSpriteFromDictionary(ref ui.Character, optionNext[progress.optionRouteFirst].character[progress.characterIndex].type, optionNext[progress.optionRouteFirst].character[progress.characterIndex].expression);
            ui.Name.text = ChangeCharacterName(optionNext[progress.optionRouteFirst].character[progress.characterIndex]);

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

        // Phase_OptionSecond�̑O����
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

        // Phase_Ending�̑O����
        public override void EndingPre()
        {
            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ui.Summary.text = optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].ending[0].type.ToString();
            ChangeCharacterSpriteFromDictionary(ref ui.Character, MainEventCharacterTypes.None, ExpressionTypes.None);
            ui.Name.text = "�G���f�B���O";

            SetPoint(optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].ending[0].type);
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

        // MainEvent�̉�b�̐i�s����
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

                    // �Ō��Character�ł͂Ȃ��ꍇ
                    if (progress.characterIndex < Characters.Length - 1)
                    {
                        // ���݂�Character�̃Z���t���Ō�ł͂Ȃ��ꍇ
                        if (progress.textIndex < Characters[progress.characterIndex].texts.Length)
                        {
                            StartCoroutinePrintCharacterText(Characters[progress.characterIndex], mainEventPhase);
                        }
                        // ���݂�Character�̃Z���t���Ō�̏ꍇ
                        else
                        {
                            ++progress.characterIndex;
                            progress.textIndex = 0;

                            ChangeCharacterSpriteFromDictionary(ref ui.Character, Characters[progress.characterIndex].type, Characters[progress.characterIndex].expression);
                            ui.Name.text = ChangeCharacterName(Characters[progress.characterIndex]);

                            StartCoroutinePrintCharacterText(Characters[progress.characterIndex], mainEventPhase);
                        }
                    }
                    // �Ō��Character�̏ꍇ
                    else
                    {
                        // �Ō�̃Z���t�ł͂Ȃ��ꍇ
                        if (progress.textIndex < Characters[progress.characterIndex].texts.Length)
                        {
                            StartCoroutinePrintCharacterText(Characters[progress.characterIndex], mainEventPhase);
                        }
                        // �Ō�̃Z���t�̏ꍇ
                        else
                        {
                            NextPhase();
                        }
                    }
                }
            }
        }

        // �O�����˓�
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

        // ��Ԗڂ̑I������
        public void OnClickFirst(int OptionRoute)
        {
            progress.optionRouteFirst = OptionRoute;
            progress.characterIndex = 0;
            progress.textIndex = 0;

            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ui.OptionParent.SetActive(false);
            ChangeCharacterSpriteFromDictionary(ref ui.Character, optionFirst[progress.optionRouteFirst].character[progress.characterIndex].type, optionFirst[progress.optionRouteFirst].character[progress.characterIndex].expression);
            ui.Name.text = ChangeCharacterName(optionFirst[progress.optionRouteFirst].character[progress.characterIndex]);

            StartCoroutinePrintCharacterText(optionFirst[progress.optionRouteFirst].character[progress.characterIndex], mainEventPhase);
        }

        // ��Ԗڂ̑I������
        public void OnClickSecond(int OptionRoute)
        {
            progress.optionRouteSecond = OptionRoute;
            progress.characterIndex = 0;
            progress.textIndex = 0;

            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ui.OptionParent.SetActive(false);
            ChangeCharacterSpriteFromDictionary(ref ui.Character, optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].character[progress.characterIndex].type, optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].character[progress.characterIndex].expression);
            ui.Name.text = ChangeCharacterName(optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].character[progress.characterIndex]);

            StartCoroutinePrintCharacterText(optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].character[progress.characterIndex], mainEventPhase);
        }

        // MainEvent�̉�b�̐i��
        public void ResetProgress(int OptionRouteFirst = -1, int OptionRouteSecond = -1, int CharacterIndex = 0, int TextIndex = 0)
        {
            progress.optionRouteFirst = OptionRouteFirst;
            progress.optionRouteSecond = OptionRouteSecond;
            progress.characterIndex = CharacterIndex;
            progress.textIndex = TextIndex;
        }

        // CharacterName��؂�ւ��鏈��
        public string ChangeCharacterName(CharacterText Character)
        {
            if (Character.type == MainEventCharacterTypes.Player)
            {
                return "�i�ߊ�";
            }
            else if (Character.type == MainEventCharacterTypes.Aside)
            {
                return "�i���[�V����";
            }
            else if (Character.type == MainEventCharacterTypes.AMA)
            {
                return Player.instance.GetCurrentAMASO().ama;
            }
            return Character.name;
        }

        // �C���X�g��؂�ւ��鏈��
        public void ChangeCharacterSpriteFromDictionary(ref Image CharacterImage, MainEventCharacterTypes CharacterType, ExpressionTypes ExpressionType)
        {
            CharacterImage.sprite = DictionaryManager.instance.GetTargetIllustration(CharacterType, ExpressionType);
            if (CharacterImage.sprite)
            {
                CharacterImage.color = HelperFunction.ChangeAlpha(CharacterImage.color, 1.0f);
            }
            else
            {
                CharacterImage.color = HelperFunction.ChangeAlpha(CharacterImage.color, 0.0f);
            }
        }

        // CharacterText�̕�������A�j���[�V������Coroutine���X�^�[�g
        public void StartCoroutinePrintCharacterText(CharacterText Character, MainEventPhase Phase)
        {
            EventUIManager.instance.StartCoroutine(StartPrintCharacterText(Character, Phase));
        }

        // Text�̕�������A�j���[�V������Coroutine���X�^�[�g
        public void StartCoroutinePrintEndingText(string Text, MainEventPhase Phase)
        {
            EventUIManager.instance.StartCoroutine(StartPrintEndingText(Text, Phase));
        }

        // CharacterText�̕�������A�j���[�V������Coroutine���X�g�b�v
        public void StopAllCoroutinePrintCharacterText(CharacterText Character, MainEventPhase Phase)
        {
            EventUIManager.instance.StopAllCoroutines();
            StopPrintCharacterText(Character, Phase);
        }

        // Text�̕�������A�j���[�V������Coroutine���X�g�b�v
        public void StopAllCoroutinePrintEndingText(string Text, MainEventPhase Phase)
        {
            EventUIManager.instance.StopAllCoroutines();
            StopPrintEndingText(Text, Phase);
        }

        // CharacterText�̕�������A�j���[�V������Coroutine
        public IEnumerator StartPrintCharacterText(CharacterText Character, MainEventPhase Phase)
        {
            onPrint = true;

            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ui.Talk.text = "";

            Sprite expression = DictionaryManager.instance.GetTargetIllustration(Character.type, Character.expression);
            if (expression)
            {
                ui.Character.sprite = expression;
            }

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

        // Text�̕�������A�j���[�V������Coroutine
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

            tweener = ui.PointText.DOFade(1.0f, frameFadeTime);
            tweener.OnUpdate(() => { ui.Point.color = HelperFunction.ChangeAlpha(ui.Point.color, ui.PointText.color.a); });
            tweener.OnComplete(() => 
            {
                ui.Point.color = HelperFunction.ChangeAlpha(ui.Point.color, ui.PointText.color.a);
                onPrint = false;
                ++progress.textIndex;
                SetNextPhase(Phase);
            });
        }

        // CharacterText�̕�������A�j���[�V������Coroutine�̒��~����
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

        // Text�̕�������A�j���[�V������Coroutine�̒��~����
        public void StopPrintEndingText(string Text, MainEventPhase Phase)
        {
            if (tweener.IsActive())
            {
                tweener.Kill();
            }

            MainEventUIElement ui = EventUIManager.instance.GetCurrentEventUI<MainEventUI>().GetEventUIElement();
            ui.Talk.text = Text;
            ui.PointText.color = HelperFunction.ChangeAlpha(ui.PointText.color, 1.0f);
            ui.Point.color = HelperFunction.ChangeAlpha(ui.Point.color, 1.0f);

            onPrint = false;
            ++progress.textIndex;

            SetNextPhase(Phase);
        }

        // MainEvent�̉�b����Character�̕\�����̌�������
        public int CheckNewExpression(CharacterText Character, int CharIndex)
        {
            string expressionType = "";
            string symbol = "�i�j";
            int charIndex = CharIndex;

            if (Character.texts[progress.textIndex][CharIndex] == symbol[0])
            {
                expressionType += Character.texts[progress.textIndex][CharIndex];
                while (Character.texts[progress.textIndex][CharIndex] != symbol[1])
                {
                    ++CharIndex;
                    expressionType += Character.texts[progress.textIndex][CharIndex];
                }

                Sprite expression = DictionaryManager.instance.GetTargetIllustration(Character.type, expressionType);
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

        public JudgementType GetJudgement()
        {
            return optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].ending[0].type;
        }

        public string GetEnding()
        {
            return optionSecond[progress.optionRouteFirst].options[progress.optionRouteSecond].ending[0].endingText;
        }
    }
}