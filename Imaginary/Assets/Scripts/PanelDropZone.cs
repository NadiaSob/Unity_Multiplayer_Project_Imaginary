using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelDropZone : NetworkBehaviour
{
    public bool AllCardsInZone = false;
    public bool CardsShuffled = false;
    public bool PointsCountedChanged = false;

    [SyncVar] public List<Transform> cards = new List<Transform>();

    private Animator animator
    { 
        get => gameObject.GetComponent<Animator>();
    }

    /*private void OnEnable()
    {
        NetworkGameManager.ActionTurnEnded += StartNewTurn;
    }

    private void OnDisable()
    {
        NetworkGameManager.ActionTurnEnded -= StartNewTurn;
    }*/

    void Update()
    {
        if (gameObject.transform.childCount == 0)
        {
            AllCardsInZone = false;
            CardsShuffled = false;
            CmdStopVote();
            CmdAllPointsNotCounted();
        }


        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        if (gameObject.transform.childCount == 4 && !AllCardsInZone)
        {
            CmdStartVote();
            //перемешиваем карты перед проигрыванием анимации
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("ShuffleCards") && !CardsShuffled)
            {
                CmdShuffleCards();
                CardsShuffled = true;
            }
            
            //запускаем анимацию
            animator.SetTrigger("PlayShuffle");
            
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("ShuffleCards") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                for (int i = 0; i < 4; ++i)
                {
                    Transform card = gameObject.transform.GetChild(i);
                    card.GetComponent<CardFlipper>().Flip();
                }

                AllCardsInZone = true;
            }
        }

        if (gameObject.transform.childCount == 4)
        {
            //проверяем, все ли очки посчитаны
            bool allPointsCounted = true;
            for (int i = 0; i < 4; ++i)
            {
                if (!gameObject.transform.GetChild(i).gameObject.GetComponent<UICard>().pointsCounted)
                {
                    allPointsCounted = false;
                }
            }
            if (allPointsCounted && !PointsCountedChanged)
            {
                Debug.Log("PanelDropZone");
                PointsCountedChanged = true;
                CmdAllPointsCounted();
            }
        }

    }

    [Command(requiresAuthority = false)]
    private void CmdStartVote()
    {
        var TurnManager = GameObject.Find("NetworkGameManager");
        var turnManager = TurnManager.GetComponent<NetworkGameManager>();
        turnManager.voteTime = true;
    }

    [Command(requiresAuthority = false)]
    private void CmdStopVote()
    {
        var TurnManager = GameObject.Find("NetworkGameManager");
        var turnManager = TurnManager.GetComponent<NetworkGameManager>();
        turnManager.voteTime = false;
    }

    [Command(requiresAuthority = false)]
    private void CmdAllPointsCounted()
    {
        var TurnManager = GameObject.Find("NetworkGameManager");
        var turnManager = TurnManager.GetComponent<NetworkGameManager>();

        if (!turnManager.endOfCourse && turnManager.assotiationExists)
        {
            turnManager.allPointsCounted = true;
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdAllPointsNotCounted()
    {
        var TurnManager = GameObject.Find("NetworkGameManager");
        var turnManager = TurnManager.GetComponent<NetworkGameManager>();
        turnManager.allPointsCounted = false;
    }

    [Server]
    private void CmdShuffleCards()
    {
        List<int> indexes = new List<int>();
        for (var i = 0; i < 4; ++i)
        {
            indexes.Add(UnityEngine.Random.Range(0, 3));
        }

        ShuffleCards(indexes);
    }

    [ClientRpc]
    private void ShuffleCards(List<int> indexes)
    {
        for (int i = 0; i < 4; ++i)
        {
            Transform card = gameObject.transform.GetChild(i);
            cards.Add(card);
        }

        for (var i = 0; i < 4; ++i)
        {
            cards[i].SetSiblingIndex(indexes[i]);
        }
        Debug.Log("ShuffleCards");
    }

    [ClientRpc]
    public void DestroyCards()
    {
        for (int i = 0; i < 4; ++i)
        {
            Destroy(gameObject.transform.GetChild(i));
        }
    }

    [ClientRpc]
    public void StartNewTurn()
    {
        

        for (int i = gameObject.transform.childCount - 1; i >= 0; --i)
        {
            //Transform card = gameObject.transform.GetChild(i);
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }

        AllCardsInZone = false;
        CardsShuffled = false;
        PointsCountedChanged = false;

        cards = new List<Transform>();
    }
}
