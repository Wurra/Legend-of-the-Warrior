using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    [Header("�¼�����")]
    public PlayAudioEventSO FXEvent;
    public PlayAudioEventSO BGMEvent;
    [Header("���")]
    public AudioSource BGMSource;
    public AudioSource FXSource;

    private void OnEnable()
    {
        FXEvent.OnEventRaised += OnFXEvent;
        BGMEvent.OnEventRaised += OnBGMEvent;
    }
    private void OnDisable()
    {
        FXEvent.OnEventRaised -= OnFXEvent;
        BGMEvent.OnEventRaised -= OnBGMEvent;
    }
    private void OnFXEvent(AudioClip audioClip)
    {
        FXSource.clip = audioClip;
        FXSource.Play();
    }
    private void OnBGMEvent(AudioClip audioClip)
    {
        
        BGMSource.clip = audioClip;
        BGMSource.Play();
    }
}

