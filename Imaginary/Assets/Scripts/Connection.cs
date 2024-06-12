using UnityEngine;
using Mirror;
using TMPro;

public class Connection : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null;
    [SerializeField] private TMP_InputField ipAddress_InputField = null;

    private void Start()
    {
        //Screen.SetResolution(800, 600, false);
    }

    public void HostLobby()
    {
        networkManager.StartHost();

        landingPagePanel.SetActive(false);
    }

    public void JoinClient()
    {
        networkManager.networkAddress = ipAddress_InputField.text;
        networkManager.StartClient();
    }
}
