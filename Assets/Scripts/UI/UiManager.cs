using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public PlayerStateBar playerStateBar;
    [Header("ÊÂ¼þ¼àÌý")]
    public CharacterEventSO healthEvent;
    public SceneLoadEventSO loadEvent;
    private void OnEnable()
    {
        healthEvent.onEventRaised += OnHealthEvent;
        loadEvent.LoadRequestEvent+= OnLoadEvent;

    }

    private void OnLoadEvent(GameSceneSO sceneToLoad, Vector3 arg1, bool arg2)
    {
        var isMenu = sceneToLoad.sceneType == SceneType.Menu;
            playerStateBar.gameObject.SetActive(!isMenu);
    }

    private void OnDisable()
    {
        healthEvent.onEventRaised-= OnHealthEvent;
        loadEvent.LoadRequestEvent-= OnLoadEvent;
    }
    private void OnHealthEvent(Character character)
    {
        var percentage=character.currentHealth/character.maxHealth;
        playerStateBar.onHealthChange(percentage);
    }
}
