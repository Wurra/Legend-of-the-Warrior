using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public PlayerStateBar playerStateBar;
    [Header("ÊÂ¼þ¼àÌý")]
    public CharacterEventSO healthEvent;
    private void OnEnable()
    {
        healthEvent.onEventRaised += OnHealthEvent;

    }
    private void OnDisable()
    {
        healthEvent.onEventRaised-= OnHealthEvent;
    }
    private void OnHealthEvent(Character character)
    {
        var percentage=character.currentHealth/character.maxHealth;
        playerStateBar.onHealthChange(percentage);
    }
}
