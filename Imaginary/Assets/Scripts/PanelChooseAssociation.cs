using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelChooseAssociation : MonoBehaviour, IPointerClickHandler
{
    [Header("UI")]
    [SerializeField] private TMP_InputField associationInputField = null;
    [SerializeField] private Button confirmButton = null;
    [SerializeField] private ButtonRevealCard buttonRevealCard = null;

    public UICard Card;

    [HideInInspector] public NetworkPlayer NetworkPlayer;

    private void Update()
    {
        confirmButton.interactable = !string.IsNullOrEmpty(associationInputField.text);
    }

    public void SaveAssociation()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        NetworkPlayer = networkIdentity.GetComponent<NetworkPlayer>();
        NetworkPlayer.CmdUpdateAssociation(associationInputField.text);

        Card.PlayCard();

        var ButtonRevealCard = GameObject.Find("Button_Reveal_Card");
        buttonRevealCard = ButtonRevealCard.GetComponent<ButtonRevealCard>();
        buttonRevealCard.Card = Card.gameObject;

        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Destroy(gameObject);
    }
}
