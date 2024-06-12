using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SocialPlatforms;

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
            }
            else
            {
                NetworkRoomPlayer.CmdChangeReadyState(true);
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
