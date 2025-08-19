using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
public class fadeCanvas : MonoBehaviour
{
    [Header("ÊÂ¼þ¼àÌý")]
    public Image fadeImage;
    public FadeEventSO fadeEvents;
    
    

    private void OnEnable()
    {
        fadeEvents.onEventRaised += onFadeEvent;
    }

    private void OnDisable()
    {
        fadeEvents.onEventRaised -= onFadeEvent;
    }

    private void onFadeEvent(Color target, float duration, bool fadeIn)
    {
        fadeImage.DOBlendableColor(target, duration);
    }
}
