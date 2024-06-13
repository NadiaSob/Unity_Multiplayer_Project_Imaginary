using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SocialPlatforms;
using TMPro;

public class RoomPlayerReadyButton : MonoBehaviour
{
    public MyNetworkRoomPlayer NetworkRoomPlayer;

    public void OnClick()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        NetworkRoomPlayer = networkIdentity.GetComponent<MyNetworkRoomPlayer>();

        if (NetworkClient.active && NetworkRoomPlayer.isLocalPlayer)
        {
            if (NetworkRoomPlayer.readyToBegin)
            {
                NetworkRoomPlayer.CmdChangeReadyState(false);
                gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Ready";
                Debug.Log(gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
                Debug.Log(gameObject.transform.GetChild(0).name);
            }
            else
            {
                NetworkRoomPlayer.CmdChangeReadyState(true);
                gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Not ready";
            }
        }

        Debug.Log(NetworkRoomPlayer.readyToBegin);
        NetworkRoomPlayer.ChangeReadyState();
    }

   // [Command]
    void ChangeReadyState()
    {
        NetworkRoomPlayer.DrawReadyState(NetworkRoomPlayer.namesManager.readyStatuses, NetworkRoomPlayer.index);
    }
}
