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
        //если подключенный клиент - это мы, то добавляем наше имя в список имен и обновляем GUI
        if (isOwned)

        {
            AddName(PlayerPrefs.GetString("PlayerName"));
            MakeShowGUI();
        }
        //если подключенный клиент - кто-то другой, то обновляем GUI
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
        //меняем статут готовности по индексу в списке статусов на противоположный
        namesManager.readyStatuses[index] = !namesManager.readyStatuses[index];
        //обновляем GUI
        ShowRoomGUI(namesManager.names, namesManager.readyStatuses);
    }

    [ClientRpc]
    public void ShowRoomGUI(List<string> names, List<bool> readyStatuses)
    {
        //проходимся циклом столько раз, сколько игроков подключено к лобби. Для каждого игрока (используя соответствующий ему индекс) печатаем имя и статус готовности
        for (int index = 0; index < names.Count; index++)
        {
            Panel_Player = GameObject.Find($"Panel_Player{index + 1}");
            TypeName(names, index);
            DrawReadyState(readyStatuses, index);
        }
    }

    public void TypeName(List<string> names, int index)
    {
        //берем первого Child от Panel_Player, а именно - панель для имени игрока
        GameObject text_name_player = Panel_Player.transform.GetChild(0).gameObject;

        //изменяем текст по умолчанию на имя того игрока из списка names, чей индекс был передан
        text_name_player.GetComponent<TextMeshProUGUI>().text = names[index];

        //text_name_player.GetComponent<TextMeshProUGUI>().text = Room.roomSlots[index].DisplayName;
    }

    public void DrawReadyState(List<bool> readyStatuses, int index)
    {
        //берем второго Child от Panel_Player, а именно - панель для статуса готовности игрока
        GameObject text_ready_player = GameObject.Find($"Panel_Player{index + 1}").transform.GetChild(1).gameObject;

        //проверяем статус готовности игрока по соответствующему индексу в списке и задаем цвет и текст статусу
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
