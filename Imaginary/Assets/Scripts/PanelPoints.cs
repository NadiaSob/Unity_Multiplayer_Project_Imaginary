using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelPoints : MonoBehaviour
{
    public PlayerType playerType;

    private GameObject TurnManager;
    private NetworkGameManager turnManager;

    private PlayerInfo enemy1Info;
    private PlayerInfo enemy2Info;
    private PlayerInfo enemy3Info;

    public TextMeshProUGUI pointsText;

    private bool pointsShowed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        //находим networkGameManager
        try
        {
            if (!turnManager)
            {
                TurnManager = GameObject.Find("NetworkGameManager");
                turnManager = TurnManager.GetComponent<NetworkGameManager>();
            }
        }
        catch (NullReferenceException ex)
        {
        }

        //инициализируем игроков
        NetworkPlayer player = NetworkPlayer.localPlayer;

        if (player)
        {
            enemy1Info = player.enemy1Info;
            enemy2Info = player.enemy2Info;
            enemy3Info = player.enemy3Info;


        }

        //если очки игрока
        if (player && playerType == PlayerType.PLAYER)
        {
            if (turnManager && turnManager.allPointsCounted && turnManager.assotiationExists)
            {
                Debug.Log("CmdSumPoints");
                turnManager.CmdSumPoints();
                /*Color color = pointsText.color;
                if (color.a < 1.0f && !pointsShowed)
                {
                    color.a += 0.005f;
                }
                else
                {
                    color.a -= 0.005f;
                    pointsShowed = true;
                    if (color.a <= 0.0f)
                    {
                        turnManager.CmdSumPoints();
                    }
                }


                if (player.Panel_Name.name == "Panel_Name_Blue")
                {
                    color = new Color(0.21f, 0.55f, 0.78f, color.a);
                }
                if (player.Panel_Name.name == "Panel_Name_Green")
                {
                    color = new Color(0.09f, 0.76f, 0.31f, color.a); ;
                }
                if (player.Panel_Name.name == "Panel_Name_Yellow")
                {
                    color = new Color(0.72f, 0.48f, 0.0f, color.a); ;
                }
                if (player.Panel_Name.name == "Panel_Name_Red")
                {
                    color = new Color(0.79f, 0.13f, 0.33f, color.a); ;
                }

                pointsText.color = color;
                pointsText.text = $"+{player.pointsOnThisCourse}";*/
            }
        }

        //если очки одного из соперников
        /*PlayerInfo enemyInfo = new PlayerInfo();
        if (player && player.hasEnemies && (playerType == PlayerType.ENEMY1 || playerType == PlayerType.ENEMY2 || playerType == PlayerType.ENEMY3))
        {
            if (playerType == PlayerType.ENEMY1)
            {
                enemyInfo = enemy1Info;
            }
            if (playerType == PlayerType.ENEMY2)
            {
                enemyInfo = enemy2Info;
            }
            if (playerType == PlayerType.ENEMY3)
            {
                enemyInfo = enemy3Info;
            }


            if (turnManager && turnManager.allPointsCounted)
            {
                Color color = pointsText.color;
                if (color.a < 1.0f && !pointsShowed)
                {
                    color.a += 0.005f;
                }
                else
                {
                    color.a -= 0.005f;
                    pointsShowed = true;
                    if (color.a <= 0.0f)
                    {
                        turnManager.CmdSumPoints();
                    }
                }

                if (enemyInfo.Panel_Name.name == "Panel_Name_Blue")
                {
                    color = new Color(0.21f, 0.55f, 0.78f, color.a);
                }
                if (enemyInfo.Panel_Name.name == "Panel_Name_Green")
                {
                    color = new Color(0.09f, 0.76f, 0.31f, color.a); ;
                }
                if (enemyInfo.Panel_Name.name == "Panel_Name_Yellow")
                {
                    color = new Color(0.72f, 0.48f, 0.0f, color.a); ;
                }
                if (enemyInfo.Panel_Name.name == "Panel_Name_Red")
                {
                    color = new Color(0.79f, 0.13f, 0.33f, color.a); ;
                }

                pointsText.color = color;
                pointsText.text = $"+{enemyInfo.pointsOnThisCourse}";
                //pointsText.text = $"<color=#ffffffff>+ {enemyInfo.pointsOnThisCourse}</color>";
            }
        }*/


    }
}
