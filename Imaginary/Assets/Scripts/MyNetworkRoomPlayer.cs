using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MyNetworkRoomPlayer : NetworkRoomPlayer
{
    public GameObject Panel_Player;
    public GameObject NamesManager;
    public GameObject Button_Ready;
    public NamesManager namesManager;

    private NetworkRoomManager room;
    private NetworkRoomManager Room
    {
        get
        {
            return room = GameObject.Find("NetworkRoomManager").GetComponent<NetworkRoomManager>();
        }
    }

    public override void OnStartServer()
    {
        NamesManager = GameObject.Find("NamesManager");
        namesManager = NamesManager.GetComponent<NamesManager>();

        //DontDestroyOnLoad(NamesManager);
    }

    public override void OnStartClient()
    {
        //���� ������������ ������ - ��� ��, �� ��������� ���� ��� � ������ ���� � ��������� GUI
        if (isOwned)

        {
            AddName(PlayerPrefs.GetString("PlayerName"));
            MakeShowGUI();
        }
        //���� ������������ ������ - ���-�� ������, �� ��������� GUI
        else
        {
            MakeShowGUI();
        }
    }

    [Command]
    public void AddName(string name)
    {
        namesManager.names.Add(name);
        namesManager.readyStatuses.Add(false);
    }

    [Command]
    public void MakeShowGUI()
    {
        ShowRoomGUI(namesManager.names, namesManager.readyStatuses);
    }

    [Command]
    public void ChangeReadyState()
    {
        //������ ������ ���������� �� ������� � ������ �������� �� ���������������
        namesManager.readyStatuses[index] = !namesManager.readyStatuses[index];
        //��������� GUI
        ShowRoomGUI(namesManager.names, namesManager.readyStatuses);
    }

    [ClientRpc]
    public void ShowRoomGUI(List<string> names, List<bool> readyStatuses)
    {
        //���������� ������ ������� ���, ������� ������� ���������� � �����. ��� ������� ������ (��������� ��������������� ��� ������) �������� ��� � ������ ����������
        for (int index = 0; index < names.Count; index++)
        {
            Panel_Player = GameObject.Find($"Panel_Player{index + 1}");
            TypeName(names, index);
            DrawReadyState(readyStatuses, index);
        }
    }

    public void TypeName(List<string> names, int index)
    {
        //����� ������� Child �� Panel_Player, � ������ - ������ ��� ����� ������
        GameObject text_name_player = Panel_Player.transform.GetChild(0).gameObject;

        //�������� ����� �� ��������� �� ��� ���� ������ �� ������ names, ��� ������ ��� �������
        text_name_player.GetComponent<TextMeshProUGUI>().text = names[index];

        //text_name_player.GetComponent<TextMeshProUGUI>().text = Room.roomSlots[index].DisplayName;
    }

    public void DrawReadyState(List<bool> readyStatuses, int index)
    {
        //����� ������� Child �� Panel_Player, � ������ - ������ ��� ������� ���������� ������
        GameObject text_ready_player = GameObject.Find($"Panel_Player{index + 1}").transform.GetChild(1).gameObject;

        //��������� ������ ���������� ������ �� ���������������� ������� � ������ � ������ ���� � ����� �������
        if (readyStatuses[index])
        {
            text_ready_player.GetComponent<TextMeshProUGUI>().color = new Color(15, 98, 230, 255);
            text_ready_player.GetComponent<TextMeshProUGUI>().text = $"<color=green>Ready</color>";
        }
        else
        {
            text_ready_player.GetComponent<TextMeshProUGUI>().color = new Color(15, 98, 230, 255);
            text_ready_player.GetComponent<TextMeshProUGUI>().text = $"<color=red>Not ready</color>";
        }
    }
}
