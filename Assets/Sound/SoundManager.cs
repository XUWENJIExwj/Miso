using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Monosingleton<SoundManager>
{

    public AudioClip[] clips;
    AudioSource audios;

    public void Init()
    {
        audios = GetComponent<AudioSource>();
    }

   //以下各種SE

   //ルート選択時
    public void SE_Route()
    {
        audios.PlayOneShot(clips[0]);
    }
   //発進ボタン
    public void SE_Go()
    { 
        audios.PlayOneShot(clips[1]);
    }
   //浄化時
   public void SE_Clean()
    {
        audios.PlayOneShot(clips[2]);
    }
   //メインイベント発生時
   public void SE_MainEvent()
    {
        audios.PlayOneShot(clips[3]);
    }
    //サブイベント発生時
    public void SE_SubEvent()
    {
        audios.PlayOneShot(clips[4]);
    }
    //会話時
    public void SE_Talk()
    {
        audios.clip = clips[5];
        audios.loop = true;
        audios.Play();
        
    }

    public void SE_StopTalk()
    {
        audios.loop = false;
        audios.Stop();
    }

    //リザルト画面の表示時
    public void SE_Result1()
    {
        audios.PlayOneShot(clips[6]);
    }
    //リザルト画面でのイベント各種の表示時
    public void SE_Result2()
    {
        audios.PlayOneShot(clips[7]);
    }

    //他タップ音
    public void SE_Tap()
    {
        audios.PlayOneShot(clips[8]);
    }
}
