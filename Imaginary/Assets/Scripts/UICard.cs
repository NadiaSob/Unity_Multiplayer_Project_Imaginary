using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICard : NetworkBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject Panel_ChooseAssociation;
    public GameObject Panel_ShowCard;
    public GameObject Panel_ChooseCard;
    public GameObject Panel_VoteForCard;

    public GameObject Vote_Dot_Blue;
    public GameObject Vote_Dot_Yellow;
    public GameObject Vote_Dot_Red;
    public GameObject Vote_Dot_Green;

    public Image CardImage;
    private NetworkPlayer localPlayer = NetworkPlayer.localPlayer;

    private GameObject NetworkGameManager;
    private NetworkGameManager networkGameManager;

    [SyncVar] public bool isPlayed = false;
    [SyncVar] public NetworkPlayer owner;
    public SyncList<NetworkPlayer> votes = new SyncList<NetworkPlayer>();

    private bool allVoted = false;

    //bools for points count
    [SyncVar] public bool pointsCounted = false;

    private void Start()
    {
        NetworkGameManager = GameObject.Find("NetworkGameManager");
        networkGameManager = NetworkGameManager.GetComponent<NetworkGameManager>();
        //Panel_ChooseAssociation = GameObject.Find("Panel_ChooseAssociation");
    }

    private void Update()
    {
        if (networkGameManager.voteTime && isPlayed)
        {
            if (!networkGameManager.allVoted && !allVoted)
            {
                bool check = true;
                foreach (NetworkPlayer player in networkGameManager.players)
                {
                    if (!player.Voted && !player.Turn)
                    {
                        check = false;
                    }
                }
                if (check)
                {
                    CmdAllVoted();
                    allVoted = true;
                    ShowVotes();
                }
            }
        }

        //включается обводка для карты ведущего
        if (networkGameManager.revealCard && owner.Turn && isPlayed)
        {
            transform.GetChild(1).GetComponent<Outline>().enabled = true;
        }
        if (networkGameManager.revealCard && isPlayed)
        {
            CmdCountPoints();
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdAllVoted()
    {
        networkGameManager.allVoted = true;
        networkGameManager.voteTime = false;
    }

    public void ShowVotes()
    {
        foreach (NetworkPlayer votePlayer in votes)
        {
            GameObject Vote_Dot = new GameObject();
            if (votePlayer.Panel_Name.gameObject.name == "Panel_Name_Blue")
            {
                Vote_Dot = Instantiate(Vote_Dot_Blue, new Vector2(0, 0), Quaternion.identity);
                Vote_Dot.transform.SetParent(gameObject.transform.GetChild(2).transform, false);
            }
            if (votePlayer.Panel_Name.gameObject.name == "Panel_Name_Yellow")
            {
                Vote_Dot = Instantiate(Vote_Dot_Yellow, new Vector2(0, 0), Quaternion.identity);
                Vote_Dot.transform.SetParent(gameObject.transform.GetChild(2).transform, false);
            }
            if (votePlayer.Panel_Name.gameObject.name == "Panel_Name_Green")
            {
                Vote_Dot = Instantiate(Vote_Dot_Green, new Vector2(0, 0), Quaternion.identity);
                Vote_Dot.transform.SetParent(gameObject.transform.GetChild(2).transform, false);
            }
            if (votePlayer.Panel_Name.gameObject.name == "Panel_Name_Red")
            {
                Vote_Dot = Instantiate(Vote_Dot_Red, new Vector2(0, 0), Quaternion.identity);
                Vote_Dot.transform.SetParent(gameObject.transform.GetChild(2).transform, false);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GetComponent<CardFlipper>().IsFront && 
            !(!localPlayer.Turn && isPlayed && !localPlayer.Voted && owner == localPlayer))
        {
            GetComponent<RectTransform>().localScale = new Vector3((float)1.5, (float)1.5, (float)1.5);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //выбор ассоциации для игрока-ведущего
        if (localPlayer.Turn && !networkGameManager.assotiationExists && GetComponent<CardFlipper>().IsFront)
        {
            //Создаем панель для выбора ассоциации
            GameObject panel_ChooseAssociation = Instantiate(Panel_ChooseAssociation, new Vector2(0, 0), Quaternion.identity);
            //Делаем панель потомком объекта Canvas
            panel_ChooseAssociation.transform.SetParent(gameObject.transform.parent.parent.transform, false);
            //Устанавливаем картинку карты в качестве картинки на панеле
            panel_ChooseAssociation.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = CardImage.sprite;
            panel_ChooseAssociation.GetComponent<PanelChooseAssociation>().Card = this;
        }

        //обычный просмотр карт
        if (((!localPlayer.Turn && !networkGameManager.assotiationExists) || (localPlayer.Turn && networkGameManager.assotiationExists) || 
            (!localPlayer.Turn && networkGameManager.assotiationExists && !localPlayer.CanChooseCard && !isPlayed) ||
            (!localPlayer.Turn && isPlayed && localPlayer.Voted)) 
            && GetComponent<CardFlipper>().IsFront)
        {
            //Создаем панель для просмотра карты
            GameObject panel_ShowCard = Instantiate(Panel_ShowCard, new Vector2(0, 0), Quaternion.identity);
            //Делаем панель потомком объекта Canvas
            panel_ShowCard.transform.SetParent(gameObject.transform.parent.parent.transform, false);
            //Устанавливаем картинку карты в качестве картинки на панеле
            panel_ShowCard.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = CardImage.sprite;
        }

        //выбор карт для игроков, не являющихся ведущими, после того, как ассоциация придумана
        if (!localPlayer.Turn && networkGameManager.assotiationExists && localPlayer.CanChooseCard && GetComponent<CardFlipper>().IsFront)
        {
            //Создаем панель для выбора карты
            GameObject panel_ChooseCard = Instantiate(Panel_ChooseCard, new Vector2(0, 0), Quaternion.identity);
            //Делаем панель потомком объекта Canvas
            panel_ChooseCard.transform.SetParent(gameObject.transform.parent.parent.transform, false);
            //Устанавливаем картинку карты в качестве картинки на панеле
            panel_ChooseCard.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = CardImage.sprite;
            //Вписываем ассоциацию в поле
            panel_ChooseCard.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = networkGameManager.Association;
            panel_ChooseCard.GetComponent<PanelChooseCard>().Card = this;
        }

        //выбор карт для голосования
        if (!localPlayer.Turn && isPlayed && GetComponent<CardFlipper>().IsFront && !localPlayer.Voted && owner != localPlayer)
        {
            GameObject panel_VoteForCard = Instantiate(Panel_VoteForCard, new Vector2(0, 0), Quaternion.identity);
            panel_VoteForCard.transform.SetParent(gameObject.transform.parent.parent.transform, false);
            panel_VoteForCard.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = CardImage.sprite;
            panel_VoteForCard.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = networkGameManager.Association;
            panel_VoteForCard.GetComponent<PanelVoteForCard>().Card = this;
        }
    }

    public void PlayCard()
    {
        gameObject.GetComponent<CardFlipper>().Flip();
        localPlayer.CmdPlayCard(gameObject);
    }

    [Command(requiresAuthority = false)]
    public void CmdAddVote(NetworkPlayer player)
    {
        votes.Add(player);
        player.Voted = true;
    }

    [Command(requiresAuthority = false)]
    public void CmdCountPoints()
    {
        if (!pointsCounted)
        {
            //если карта ведущего
            if (owner.Turn)
            {
                //если все проголосовали за карту ведущего
                if (votes.Count == 3)
                {
                    //каждый игрок, кроме ведущего, получает по 3 очка
                    foreach (NetworkPlayer player in votes)
                    {
                        player.pointsOnThisCourse += 3;
                    }
                }
                else if (votes.Count != 0)
                {
                    //базовые три очка ведущему
                    owner.pointsOnThisCourse += 3;

                    foreach (NetworkPlayer player in votes)
                    {
                        owner.pointsOnThisCourse += 1;
                        player.pointsOnThisCourse += 3;
                    }
                }
            }
            //если карта одного из игроков
            else
            {
                owner.pointsOnThisCourse += votes.Count;
            }

            pointsCounted = true;
        }
    }
}
