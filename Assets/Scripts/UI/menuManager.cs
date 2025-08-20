using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class menuManager : MonoBehaviour
{
    public GameObject newGameButton;
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(newGameButton);
    }
    public void ExitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }
}
