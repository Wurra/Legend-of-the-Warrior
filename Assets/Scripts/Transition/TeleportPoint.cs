using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour,IInteractable
{
    public GameSceneSO sceneToGo;
    public SceneLoadEventSO LoadEventSO;
    public Vector3 positionToGo;
    public void TriggerAction()
    {
        Debug.Log("传送点被触发");
        LoadEventSO.RaiseLoadRequestEvent(sceneToGo, positionToGo, true);
    }


}
