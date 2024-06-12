using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelName : MonoBehaviour
{
    public GameObject panel;
    //public Image portrait;
    public TextMeshProUGUI username;
    public TextMeshProUGUI points;
    public GameObject Panel_Name;
    public PlayerType playerType;

    private PlayerInfo enemy1Info;
    private PlayerInfo enemy2Info;
    private PlayerInfo enemy3Info;

    private bool isPanelDisplayed = false;

    void Update()
    {
        NetworkPlayer player = NetworkPlayer.localPlayer;

        if (player)
        {
            enemy1Info = player.enemy1Info;
            enemy2Info = player.enemy2Info;
            enemy3Info = player.enemy3Info;
        }

        if (player && playerType == PlayerType.PLAYER)
        {
            if (!isPanelDisplayed)
            {
                Panel_Name = Instantiate(player.Panel_Name, new Vector2(0, 0), Quaternion.identity);
                Panel_Name.transform.SetParent(panel.transform, false);
                isPanelDisplayed = true;
            }
            Panel_Name.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = player.username;
            Panel_Name.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = player.points.ToString();

            //когда игрок голосует за карточку, отображается значок голоса рядом с панелью
            if (player.Voted)
            {
                Panel_Name.transform.GetChild(4).gameObject.SetActive(true);
            }
            else
            {
                Panel_Name.transform.GetChild(4).gameObject.SetActive(false);
            }

            if (player.Turn)
            {
                Panel_Name.transform.GetChild(0).gameObject.SetActive(true);
                //Panel_Name.GetComponent<Outline>().enabled = true;
            }
            else
            {
                Panel_Name.transform.GetChild(0).gameObject.SetActive(false);
                //Panel_Name.GetComponent<Outline>().enabled = false;
            }
        }

        PlayerInfo enemyInfo = new PlayerInfo();
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

            if (!isPanelDisplayed)
            {
                Panel_Name = Instantiate(enemyInfo.Panel_Name, new Vector2(0, 0), Quaternion.identity);
                Panel_Name.transform.SetParent(panel.transform, false);
                isPanelDisplayed = true;
            }
            Panel_Name.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = enemyInfo.username;
            Panel_Name.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = enemyInfo.points.ToString();
            //когда игрок голосует за карточку, отображается значок голоса рядом с панелью
            if (enemyInfo.voted)
            {
                Panel_Name.transform.GetChild(4).gameObject.SetActive(true);
            }
            else
            {
                Panel_Name.transform.GetChild(4).gameObject.SetActive(false);
            }

            //подсвечиваем имя игрока, если сейчас его ход
            if (enemyInfo.turn)
            {
                Panel_Name.transform.GetChild(0).gameObject.SetActive(true);
                //Panel_Name.GetComponent<Outline>().enabled = true;
            }
            else
            {
                Panel_Name.transform.GetChild(0).gameObject.SetActive(false);
                //Panel_Name.GetComponent<Outline>().enabled = false;
            }

            
        }
    }
}
