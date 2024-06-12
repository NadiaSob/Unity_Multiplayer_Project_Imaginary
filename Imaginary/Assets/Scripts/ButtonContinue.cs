using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Mirror;

public class ButtonContinue : NetworkBehaviour
{
    private GameObject TurnManagerObject;
    private NetworkGameManager turnManager;

    public bool isShown = false;

    // Update is called once per frame
    void Update()
    {
        //инициализируем networkGameManager
        try
        {
            if (!turnManager)
            {
                TurnManagerObject = GameObject.Find("NetworkGameManager");
                turnManager = TurnManagerObject.GetComponent<NetworkGameManager>();
            }
        }
        catch (NullReferenceException ex)
        {
        }

        /*if (turnManager && turnManager.endOfCourse && !isShown)
        {
            gameObject.GetComponent<Image>().enabled = true;
            transform.GetChild(0).gameObject.SetActive(true);
            isShown = true;
        }

        if (turnManager && !turnManager.endOfCourse && isShown)
        {
            gameObject.GetComponent<Image>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
            isShown = false;
        }*/

    }

    public void ShowContinueButton()
    {
        Debug.Log("ShowContinueButton");
        gameObject.GetComponent<Image>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
        isShown = true;
    }

    public void ContinueGame()
    {
        Debug.Log("ContinueGame");
        turnManager.CmdReadyToContinuePlayer(NetworkPlayer.localPlayer);
    }

    [ClientRpc]
    public void StartNewTurn()
    {
        isShown = false;
    }

}
