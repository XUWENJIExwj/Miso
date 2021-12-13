using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public enum MetalFrameState
{
    Play,
    Repeat,
    Stop,
    None,
}

public class AchievementMetalFrame : MonoBehaviour
{
    [SerializeField] private Image frame = null;
    [SerializeField] private MetalFrameState state = MetalFrameState.None;
    [SerializeField] private bool mouseIn = false;

    public void Hide()
    {
        frame.color = HelperFunction.ChangeAlpha(frame.color, 0.0f);
    }

    public void SetColor(Color Color)
    {
        frame.color = Color;
    }

    public void SetMouseIn(bool MouseIn)
    {
        mouseIn = MouseIn;
    }

    public void SetAnimateState(MetalFrameState State)
    {
        state = State;
    }

    public void StartAnimateFrame()
    {
        SetMouseIn(true);

        if (state == MetalFrameState.None)
        {
            Play();
        }

    }

    private void Play()
    {
        StartCoroutine(AnimateFrame());
    }

    private IEnumerator AnimateFrame(float Time = 1.0f)
    {
        SetAnimateState(MetalFrameState.Play);

        Sprite[] frames = AchievementView.instance.GetMetalFrames();
        float interval = Time / frames.Length;

        for (int i = 0; i < frames.Length; ++i)
        {
            frame.sprite = frames[i];
            yield return new WaitForSeconds(interval);
        }

        if (mouseIn && state == MetalFrameState.Play)
        {
            SetAnimateState(MetalFrameState.Repeat);
            yield return StartCoroutine(Repeat(frames, interval));
        }
        else
        {
            SetAnimateState(MetalFrameState.None);

            if (mouseIn)
            {
                SetAnimateState(MetalFrameState.Repeat);
                yield return StartCoroutine(Repeat(frames, interval));
            }
        }
    }

    private IEnumerator Repeat(Sprite[] Frames, float Interval)
    {
        for (int i = 0; i < Frames.Length; ++i)
        {
            frame.sprite = Frames[i];
            yield return new WaitForSeconds(Interval);
        }

        if (mouseIn && state == MetalFrameState.Repeat)
        {
            yield return StartCoroutine(Repeat(Frames, Interval));
        }
        else
        {
            SetAnimateState(MetalFrameState.None);

            if (mouseIn)
            {
                SetAnimateState(MetalFrameState.Repeat);
                yield return StartCoroutine(Repeat(Frames, Interval));
            }
        }
    }

    public void StopAnimateFrame()
    {
        SetMouseIn(false);
        SetAnimateState(MetalFrameState.Stop);
    }

    public void OnDisable()
    {
        SetAnimateState(MetalFrameState.None);
    }
}
