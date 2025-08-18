using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour,IInteractable
{
    private SpriteRenderer spriteRenderer;
    public Sprite openSprite;
    public Sprite closedSprite;
    public bool isDone;
    public GameObject key;
    public void TriggerAction()
    {
        if (!isDone)
        {
            OpenChest();
            Instantiate(key, transform.position, Quaternion.identity);
        }
    }
    private void Awake()
    {
        spriteRenderer=GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        spriteRenderer.enabled=isDone?openSprite:closedSprite;
    }
  
    private void OpenChest()
    {
        isDone = true;
        spriteRenderer.sprite = openSprite;
        
    }
}
