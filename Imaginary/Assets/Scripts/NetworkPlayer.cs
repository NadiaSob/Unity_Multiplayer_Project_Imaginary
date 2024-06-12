using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Threading;
using TMPro;
using UnityEngine.SocialPlatforms;

public enum PlayerType { PLAYER, ENEMY1, ENEMY2, ENEMY3 };

public class NetworkPlayer : NetworkBehaviour
{
    //public List<GameObject> Cards;

    public GameObject CardBack;
    public GameObject Panel_Drop_Zone;
    public GameObject Panel_Hands;
    public GameObject Panel_Enemy1;
    public GameObject Panel_Enemy2;
    public GameObject Panel_Enemy3;
    public GameObject NetworkGameManager;

    public GameObject Panel_Name_Blue;
    public GameObject Panel_Name_Green;
    public GameObject Panel_Name_Purple;
    public GameObject Panel_Name_Red;
    public GameObject Panel_Name_Yellow;

    public GameObject Panel_Association;

    [SyncVar(hook = nameof(UpdatePlayerName))] public string username;
    [SyncVar] public int points;
    [SyncVar] public int pointsOnThisCourse;

    [SyncVar] public bool readyToContinue;

    //[SyncVar] public bool assotiationExists = false;
    //[SyncVar] public string Association;

    public GameObject Panel_Name
    { 
        get
        {
            switch (LocalID)
            {
                case 0:
                    return Panel_Name_Blue;
                case 1:
                    return Panel_Name_Yellow;
                case 2:
                    return Panel_Name_Red;
                case 3:
                    return Panel_Name_Green;
            }

            return null;
        }
    }

    [HideInInspector] public PlayerInfo enemy1Info;
    [HideInInspector] public PlayerInfo enemy2Info;
    [HideInInspector] public PlayerInfo enemy3Info;
    [HideInInspector] public static NetworkPlayer localPlayer;
    [HideInInspector] public bool hasEnemies = false; // If we have set enemies

    [SyncVar] public bool Turn;
    [SyncVar] public bool Voted;
    public bool CanChooseCard;

    public int LocalID;
    public NetworkGameManager networkGameManager;

    public override void OnStartLocalPlayer()
    {
        localPlayer = this;
        Debug.Log(LocalID);
        points = 0;
        pointsOnThisCourse = 0;
        readyToContinue = false;



        // раскомментировать!!
        CmdLoadName(PlayerPrefs.GetString("PlayerName"));

        /*if (LocalID == 0)
        {
            CmdLoadName("Oleg");
        }
        if (LocalID == 1)
        {
            CmdLoadName("Nadia");
        }
        if (LocalID == 2)
        {
            CmdLoadName("Lera");
        }
        if (LocalID == 3)
        {
            CmdLoadName("Andrew");
        }*/
    }

    /*public override void OnStartServer()
    {
        
    }*/

    public override void OnStartClient()
    {

        Panel_Drop_Zone = GameObject.Find("Panel_Drop_Zone");
        Panel_Hands = GameObject.Find("Panel_Hands");
        Panel_Enemy1 = GameObject.Find("Panel_Enemy_1");
        Panel_Enemy2 = GameObject.Find("Panel_Enemy_2");
        Panel_Enemy3 = GameObject.Find("Panel_Enemy_3");
        Panel_Association = GameObject.Find("Panel_Association");
        //Panel_Association.SetActive(false);

        NetworkGameManager = GameObject.Find("NetworkGameManager");
        networkGameManager = NetworkGameManager.GetComponent<NetworkGameManager>();

        //networkGameManager.turnID = 0;

        AddPlayer();

        if (LocalID == 0)
        {
            Turn = true;
        }
        else
        {
            Turn = false;
        }
        CanChooseCard = false;
        Voted = false;
    }

    public void Update()
    {
        UpdateEnemiesInfo();

        /*if (networkGameManager.allReadyToContinue)
        {
            if (Panel_Drop_Zone)
            {
                Panel_Drop_Zone.GetComponent<PanelDropZone>().StartNewTurn();
                Debug.Log("Update in NetwPlayer");
            }
        }*/
        
    }

    public void UpdateEnemiesInfo()
    {
        if (networkGameManager.players.Count == 4)
        {
            foreach (var player in networkGameManager.players)
            {
                PlayerInfo currentPlayer = new PlayerInfo(player.gameObject);

                switch (LocalID)
                {
                    case 0:
                        if (player.LocalID == 1)
                        {
                            enemy1Info = currentPlayer;
                        }
                        if (player.LocalID == 2)
                        {
                            enemy2Info = currentPlayer;
                        }
                        if (player.LocalID == 3)
                        {
                            enemy3Info = currentPlayer;
                        }
                        break;
                    case 1:
                        if (player.LocalID == 2)
                        {
                            enemy1Info = currentPlayer;
                        }
                        if (player.LocalID == 3)
                        {
                            enemy2Info = currentPlayer;
                        }
                        if (player.LocalID == 0)
                        {
                            enemy3Info = currentPlayer;
                        }
                        break;
                    case 2:
                        if (player.LocalID == 3)
                        {
                            enemy1Info = currentPlayer;
                        }
                        if (player.LocalID == 0)
                        {
                            enemy2Info = currentPlayer;
                        }
                        if (player.LocalID == 1)
                        {
                            enemy3Info = currentPlayer;
                        }
                        break;
                    case 3:
                        if (player.LocalID == 0)
                        {
                            enemy1Info = currentPlayer;
                        }
                        if (player.LocalID == 1)
                        {
                            enemy2Info = currentPlayer;
                        }
                        if (player.LocalID == 2)
                        {
                            enemy3Info = currentPlayer;
                        }
                        break;
                }
                hasEnemies = true;
            }
        }
    }

    // Update the localPlayer's username, as well as the box above the localPlayer's head where their name is displayed.
    void UpdatePlayerName(string oldUser, string newUser)
    {
        // Update username
        username = newUser;

        // Update game object's name in editor (only useful for debugging).
        //gameObject.name = newUser;
    }

    //раздаем карты
    [Command]
    public void CmdDealCards(NetworkPlayer owner, int count)
    {
        for (int i = 0; i < count; i++)
        {
            var randCard = networkGameManager.Cards[Random.Range(0, networkGameManager.Cards.Count)];
            

            GameObject card = Instantiate(randCard, new Vector2(0, 0), Quaternion.identity);
            NetworkServer.Spawn(card, connectionToClient);
            networkGameManager.CmdRemoveCardFromDeck(randCard);

            card.GetComponent<UICard>().owner = owner;

            ShowCards(card);
        }
    }

    [Command]
    public void CmdLoadName(string name)
    {
        username = name;
    }

    [Command]
    public void CmdPlayCard(GameObject Card)
    {
        Card.GetComponent<UICard>().isPlayed = true;
        MoveCardToDropZone(Card);
    }

    [ClientRpc]
    public void MoveCardToDropZone(GameObject Card)
    {
        Card.transform.SetParent(Panel_Drop_Zone.transform, false);
    }

    [Command]
    public void CmdUpdateAssociation(string association)
    {
        networkGameManager.Association = association;
        networkGameManager.assotiationExists = true;

        ShowAssociation(association);
    }

    [ClientRpc]
    public void ShowAssociation(string association)
    {
        Panel_Association.transform.GetChild(0).gameObject.SetActive(true);
        Debug.Log("ShowAssociation");
        Panel_Association.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = association;
        foreach (var player in networkGameManager.players)
        {
            player.CanChooseCard = true;
        }
    }

    [ClientRpc]
    public void StartNewTurn(bool lastTurn)
    {
        Panel_Association.transform.GetChild(0).gameObject.SetActive(false);
        Panel_Association.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "";
        foreach (var player in networkGameManager.players)
        {
            player.CanChooseCard = false;
            player.Voted = false;
            pointsOnThisCourse = 0;
            readyToContinue = false;
        }

        if (!lastTurn)
        {
            CmdDealCards(this, 1);
        }
        //добавить логику окончания игры
    }


    [ClientRpc]
    public void ShowCards(GameObject card)
    {
        if (isOwned)
        {
            card.transform.SetParent(Panel_Hands.transform, false);
        }
        else
        {
            int localID_owned = 0;
            foreach (var player in networkGameManager.players)
            {
                if (player.isOwned)
                {
                    localID_owned = player.LocalID;
                }
            }
            switch (localID_owned)
            {
                    case 0: 
                        if (LocalID == 1)
                        {
                            card.transform.SetParent(Panel_Enemy1.transform, false);
                        }
                        if (LocalID == 2)
                        {
                            card.transform.SetParent(Panel_Enemy2.transform, false);
                        }
                        if (LocalID == 3)
                        {
                            card.transform.SetParent(Panel_Enemy3.transform, false);
                        }
                        break;
                    case 1:
                        if (LocalID == 2)
                        {
                            card.transform.SetParent(Panel_Enemy1.transform, false);
                        }
                        if (LocalID == 3)
                        {
                            card.transform.SetParent(Panel_Enemy2.transform, false);
                        }
                        if (LocalID == 0)
                        {
                            card.transform.SetParent(Panel_Enemy3.transform, false);
                        }
                        break;
                    case 2:
                        if (LocalID == 3)
                        {
                            card.transform.SetParent(Panel_Enemy1.transform, false);
                        }
                        if (LocalID == 0)
                        {
                            card.transform.SetParent(Panel_Enemy2.transform, false);
                        }
                        if (LocalID == 1)
                        {
                            card.transform.SetParent(Panel_Enemy3.transform, false);
                        }
                        break;
                    case 3:
                        if (LocalID == 0)
                        {
                            card.transform.SetParent(Panel_Enemy1.transform, false);
                        }
                        if (LocalID == 1)
                        {
                            card.transform.SetParent(Panel_Enemy2.transform, false);
                        }
                        if (LocalID == 2)
                        {
                            card.transform.SetParent(Panel_Enemy3.transform, false);
                        }
                        break;
            }

            //перевернуть карты
            card.GetComponent<CardFlipper>().Flip();
        }
        
    }

    public void AddPlayer()
    {
        networkGameManager.players.Add(this);
        Debug.Log(networkGameManager.players.Count);
        LocalID = networkGameManager.players.IndexOf(this);
    }
}
