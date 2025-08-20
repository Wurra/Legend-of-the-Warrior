using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointRock : MonoBehaviour, IInteractable
{
    public SpriteRenderer spriteRenderer;
    public Sprite openSprite;
    public Sprite closedSprite;
    public bool isDone;
    
    public void TriggerAction()
    {
        if (!isDone)
        {
            Save();          
        }
    }
    private void OnEnable()
    {
        spriteRenderer.enabled = isDone ? openSprite : closedSprite;
    }


    private void Save()
    {
        isDone = true;
        spriteRenderer.sprite = openSprite;
        this.gameObject.tag = "Untagged";
    }
}
