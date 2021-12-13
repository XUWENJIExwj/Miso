using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBGM : Monosingleton<ChangeBGM>
{
    // Start is called before the first frame update
    public AudioClip[] clips;
    AudioSource audios;

    public void Init()
    {
        audios = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void BGM_Route()
    {
        audios.clip = clips[0];
        audios.Play();
    }

    //‘¼ƒ^ƒbƒv‰¹
    public void BGM_Move()
    {
        audios.clip = clips[1];
        audios.Play();
    }}
