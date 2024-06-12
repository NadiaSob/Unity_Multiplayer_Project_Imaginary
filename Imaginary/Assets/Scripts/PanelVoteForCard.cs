using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelVoteForCard : MonoBehaviour, IPointerClickHandler
{
    public UICard Card;

    public void OnPointerClick(PointerEventData eventData)
    {
        Destroy(gameObject);
    }

    public void VoteForCard()
    {
        NetworkPlayer player = NetworkPlayer.localPlayer;
        Card.CmdAddVote(player);

        Destroy(gameObject);
    }
}
