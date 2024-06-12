using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanelChooseCard : MonoBehaviour, IPointerClickHandler
{
    public UICard Card;

    public void OnPointerClick(PointerEventData eventData)
    {
        Destroy(gameObject);
    }

    public void ChooseCard()
    {
        NetworkPlayer player = NetworkPlayer.localPlayer;
        player.CanChooseCard = false;

        Card.PlayCard();
        Destroy(gameObject);
    }
}
