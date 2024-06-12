using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[Serializable]
public partial struct PlayerInfo
{
    public GameObject player;

    public PlayerInfo(GameObject player)
    {
        this.player = player;
    }

    public NetworkPlayer data
    {
        get
        {
            // Return ScriptableItem from our cached list, based on the card's uniqueID.
            return player.GetComponentInChildren<NetworkPlayer>();
        }
    }

    // Player's username
    public string username => data.username;
    //public Sprite portrait => data.portrait;

    public int points => data.points;

    public int pointsOnThisCourse => data.pointsOnThisCourse;

    public bool turn => data.Turn;

    public GameObject Panel_Name => data.Panel_Name;

    public bool voted => data.Voted;
}
