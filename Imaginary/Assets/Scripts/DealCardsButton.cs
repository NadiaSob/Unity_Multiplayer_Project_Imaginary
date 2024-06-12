using UnityEngine;
using Mirror;

public class DealCardsButton : MonoBehaviour
{
    public NetworkPlayer NetworkPlayer;

    public void OnClick()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        NetworkPlayer = networkIdentity.GetComponent<NetworkPlayer>();
        NetworkPlayer.CmdDealCards(NetworkPlayer.localPlayer, 6);

        gameObject.SetActive(false);
    }
}
