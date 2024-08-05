using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Utp;

public class MyNetworkRoomPlayer : NetworkRoomPlayer
{
    public GameObject Panel_Player;
    public GameObject NamesManager;
    public GameObject Button_Ready;
    public NamesManager namesManager;
    public GameObject Panel_JoinCode;

    [SerializeField] private Mirror.NetworkManager networkManager = null;
    private RelayManager _relayManager;
    private NetworkRoomManager room;
    private NetworkRoomManager Room
    {
        get
        {
            return room = GameObject.Find("NetworkRoomManager").GetComponent<NetworkRoomManager>();
        }
    }

    public void Update()
    {
        if (Panel_JoinCode)
        {
            Panel_JoinCode.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = _relayManager.JoinCode;
        }
    }

    public override void OnStartServer()
    {
        NamesManager = GameObject.Find("NamesManager");
        namesManager = NamesManager.GetComponent<NamesManager>();

        Panel_JoinCode = GameObject.Find("Panel_JoinCode");
        Panel_JoinCode.GetComponent<Image>().enabled = true;
        Panel_JoinCode.transform.GetChild(0).gameObject.SetActive(true);
        Panel_JoinCode.transform.GetChild(1).gameObject.SetActive(true);
        Panel_JoinCode.transform.GetChild(2).gameObject.SetActive(true);

        networkManager = GameObject.Find("NetworkRoomManager").GetComponent<NetworkRoomManager>();
        _relayManager = networkManager.GetComponent<RelayManager>();

        Panel_JoinCode.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = _relayManager.JoinCode;


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

    [Mirror.ClientRpc]
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
        GameObject panel_ReadyStatus = GameObject.Find($"Panel_Player{index + 1}").transform.GetChild(1).gameObject;

        //��������� ������ ���������� ������ �� ���������������� ������� � ������ � ���������� ������ ����� � ������
        if (readyStatuses[index])
        {
            panel_ReadyStatus.transform.GetChild(1).GetComponent<Image>().enabled = true;
            panel_ReadyStatus.transform.GetChild(0).GetComponent<Image>().enabled = false;
        }
        else
        {
            panel_ReadyStatus.transform.GetChild(1).GetComponent<Image>().enabled = false;
            panel_ReadyStatus.transform.GetChild(0).GetComponent<Image>().enabled = true;
        }
    }
}
