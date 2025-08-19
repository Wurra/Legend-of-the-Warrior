using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Event/FadeEventSO")]
public class FadeEventSO : ScriptableObject
{
    public UnityAction<Color, float, bool> onEventRaised;
    //Öð½¥±äºÚ
    public void FadeIn(float duration)
    {
        RaiseEvent(Color.black, duration, true);
    }
    public void FadeOut(float duration) 
    {
        RaiseEvent(Color.clear, duration, false);

    }
    public void RaiseEvent(Color targetColor, float duration, bool isFadeIn)
    {
        if (onEventRaised != null)
        {
            onEventRaised.Invoke(targetColor, duration, isFadeIn);
        }
    }   
}
