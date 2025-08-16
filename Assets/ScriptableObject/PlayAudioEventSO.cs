
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/PlayAudioEventSO")]
public class PlayAudioEventSO : ScriptableObject
{
   public UnityAction<AudioClip> OnEventRaised;
    //����һ���¼�ί�У�������AudioClip������
    //�����ű����Զ�������¼�����Ӧ��Ƶ��������
    public void RaiseEvent(AudioClip audioClip)
    {
        if (OnEventRaised != null)
        {
            OnEventRaised.Invoke(audioClip);
        }
    }
}
