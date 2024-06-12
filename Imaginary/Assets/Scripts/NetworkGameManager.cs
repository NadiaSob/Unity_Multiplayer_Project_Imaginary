using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NetworkGameManager : NetworkBehaviour
{
    public List<NetworkPlayer> players = new List<NetworkPlayer>();

    //public int turnID;

    //public static Action ActionTurnEnded;

    public List<GameObject> CardsList;
    public SyncList<GameObject> Cards = new SyncList<GameObject>();

    public GameObject ButtonContinue;

    [SyncVar] public bool assotiationExists = false;
    [SyncVar] public string Association;
    [SyncVar] public bool voteTime = false;
    [SyncVar] public bool allVoted = false;
    [SyncVar] public bool revealCard = false;
    [SyncVar] public bool allPointsCounted = false;
    [SyncVar] public bool endOfCourse = false;
    //[SyncVar] public bool allReadyToContinue = false;

    public override void OnStartServer()
    {
        //NetworkPlayer localPlayer = NetworkPlayer.localPlayer;
        
        foreach (var card in CardsList)
        {
            Cards.Add(card);
        }
    }

    /*[Server]
    private void Update()
    {
        if (allReadyToContinue)
        {
            CmdStartNewCourse();
            allReadyToContinue = false;
        }
    }*/

    [Command(requiresAuthority = false)]
    public void CmdSumPoints()
    {
        foreach (NetworkPlayer player in players)
        {
            player.points += player.pointsOnThisCourse;
            player.pointsOnThisCourse = 0;
        }

        //ButtonContinue.ShowContinueButton();
        allPointsCounted = false;
        ContinueGame();
        endOfCourse = true;
        
        
    }

    [ClientRpc]
    public void ContinueGame()
    {
        
        ButtonContinue = GameObject.Find("Button_Continue");
        Debug.Log(ButtonContinue);
        ButtonContinue.GetComponent<ButtonContinue>().ShowContinueButton();
    }

    //[Command(requiresAuthority = false)]
    public void CmdRemoveCardFromDeck(GameObject card)
    {
        Cards.Remove(card);
    }

    [Command(requiresAuthority = false)]
    public void CmdReadyToContinuePlayer(NetworkPlayer readyPlayer)
    {
        readyPlayer.readyToContinue = true;

        bool allReady = true;
        foreach (NetworkPlayer player in players)
        {
            if (!player.readyToContinue)
            {
                allReady = false;
            }
        }
        if (allReady)
        {
            CmdStartNewCourse();
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdStartNewCourse()
    {
        //меняем ход игрока на следующего в списке
        for (int i = 0; i < 4; ++i)
        {
            if (players[i].Turn)
            {
                if (i + 1 < 4)
                {
                    players[i].Turn = false;
                    players[i + 1].Turn = true;
                    break;
                }
                else
                {
                    players[i].Turn = false;
                    players[0].Turn = true;
                    break;
                }
            }
        }

        assotiationExists = false;
        Association = "";
        voteTime = false;
        allVoted = false;
        revealCard = false;
        allPointsCounted = false;
        endOfCourse = false;


        var PanelDropZone = GameObject.Find("Panel_Drop_Zone");
        var panelDropZone = PanelDropZone.GetComponent<PanelDropZone>();
        panelDropZone.StartNewTurn();

        var ButtonContinue = GameObject.Find("Button_Continue");
        var buttonContinue = ButtonContinue.GetComponent<ButtonContinue>();
        buttonContinue.StartNewTurn();

        if (Cards.Count >= 4)
        {
            foreach (NetworkPlayer player in players)
            {
                player.StartNewTurn(false);
            }
        }
        else
        {
            foreach (NetworkPlayer player in players)
            {
                player.StartNewTurn(true);
            }
        }
    }


}
