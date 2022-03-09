using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeTrigger : MonoBehaviour
{
    public GameObject gameManager;
    private GameManager gameManagerScript;
    private bool endedGame;

    public void Start()
    {
        
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
        endedGame = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("Collided with: " + other);
        if(endedGame == false){
            gameManagerScript.EndGame();
            endedGame = true;
        }
    }
}
