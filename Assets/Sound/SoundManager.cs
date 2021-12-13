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

   //�ȉ��e��SE

   //���[�g�I����
    public void SE_Route()
    {
        audios.PlayOneShot(clips[0]);
    }
   //���i�{�^��
    public void SE_Go()
    { 
        audios.PlayOneShot(clips[1]);
    }
   //�򉻎�
   public void SE_Clean()
    {
        audios.PlayOneShot(clips[2]);
    }
   //���C���C�x���g������
   public void SE_MainEvent()
    {
        audios.PlayOneShot(clips[3]);
    }
    //�T�u�C�x���g������
    public void SE_SubEvent()
    {
        audios.PlayOneShot(clips[4]);
    }
    //��b��
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

    //���U���g��ʂ̕\����
    public void SE_Result1()
    {
        audios.PlayOneShot(clips[6]);
    }
    //���U���g��ʂł̃C�x���g�e��̕\����
    public void SE_Result2()
    {
        audios.PlayOneShot(clips[7]);
    }

    //���^�b�v��
    public void SE_Tap()
    {
        audios.PlayOneShot(clips[8]);
    }
}
