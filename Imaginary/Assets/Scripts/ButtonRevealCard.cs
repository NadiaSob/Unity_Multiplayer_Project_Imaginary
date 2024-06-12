using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonRevealCard : NetworkBehaviour
{
    private GameObject TurnManager;
    private NetworkGameManager turnManager;

    public GameObject Card;

    private void Update()
    {
        try
        {
            if (!turnManager)
            {
                TurnManager = GameObject.Find("NetworkGameManager");
                turnManager = TurnManager.GetComponent<NetworkGameManager>();
            }
        }
        catch (NullReferenceException ex)
        { 
            //Debug.LogError(ex.ToString());
        }

        if (turnManager && turnManager.allVoted)
        {
            NetworkPlayer player = NetworkPlayer.localPlayer;
            if (player.Turn)
            {
                gameObject.GetComponent<Image>().enabled = true;
                transform.GetChild(0).gameObject.SetActive(true);
                CmdAllVotedDone();
            }
        }
    }


    public void RevealCard()
    {
        //Card.transform.GetChild(1).GetComponent<Outline>().enabled = true;
        gameObject.GetComponent<Image>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);

        CmdRevealCard();
        
    }

    [Command(requiresAuthority = false)]
    public void CmdAllVotedDone()
    {
        turnManager.allVoted = false;
    }

    [Command(requiresAuthority = false)]
    public void CmdRevealCard()
    {
        turnManager.revealCard = true;

    }

}
