using System;
using Mirror;
using TMPro;
using UnityEngine;
using Utp;

[RequireComponent(typeof(RelayManager))]
public class Connection : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null;
    [SerializeField] private TMP_InputField ipAddress_InputField = null;
    //[SerializeField] private TextMeshProUGUI joinCodeText = null;

    private RelayManager _relayManager;

    private void Start()
    {
        //Screen.SetResolution(800, 600, false);
        _relayManager = networkManager.GetComponent<RelayManager>();
    }

    public void HostLobby()
    {
        try
        {
            StartRelayHost(4);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

        // networkManager.StartHost();
        // landingPagePanel.SetActive(false);
    }

    public void StartRelayHost(int maxPlayers, string regionId = null)
    {
        _relayManager.AllocateRelayServer(maxPlayers, regionId,
            joinCode =>
            {
                Debug.LogError($"Join by code: {joinCode}");
                _relayManager.JoinCode = joinCode;
                //joinCodeText.text = joinCode;
                networkManager.StartHost();
            },
            () =>
            {
                UtpLog.Error($"Failed to start a Relay host.");
            });
    }

    public void JoinClient()
    {
        string joinCode = ipAddress_InputField.text;
        _relayManager.GetAllocationFromJoinCode(joinCode,
            () =>
            {
                networkManager.StartClient();
            },
            () =>
            {
                UtpLog.Error($"Failed to join Relay server.");
            });

        // networkManager.networkAddress = ipAddress_InputField.text;
        // networkManager.StartClient();
    }
}