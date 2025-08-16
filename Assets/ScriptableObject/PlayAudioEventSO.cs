
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/PlayAudioEventSO")]
public class PlayAudioEventSO : ScriptableObject
{
   public UnityAction<AudioClip> OnEventRaised;
    //声明一个事件委托，它接受AudioClip参数。
    //其他脚本可以订阅这个事件来响应音频播放请求
    public void RaiseEvent(AudioClip audioClip)
    {
        if (OnEventRaised != null)
        {
            OnEventRaised.Invoke(audioClip);
        }
    }
}
