using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using Unity.Multiplayer.Tools;
using System.IO;
using System.Text;
using System.Net;
using TMPro;


public class Main : NetworkBehaviour {

    public Button btnHost;
    public Button btnClient;
    public TMPro.TMP_Text txtStatus;
    public IPAddress ips;

    public void Start() {
        btnHost.onClick.AddListener(OnHostClicked);
        btnClient.onClick.AddListener(OnClientClicked);
        Application.targetFrameRate = 30;
    }

    private void StartHost() {
        NetworkManager.Singleton.StartHost();
        NetworkManager.SceneManager.LoadScene(
            "Lobby",
            UnityEngine.SceneManagement.LoadSceneMode.Single);
    }



    private void OnHostClicked() {
        btnClient.gameObject.SetActive(false);
        btnHost.gameObject.SetActive(false);
        txtStatus.text = "Starting Host";
        StartHost();
    }

    private void OnClientClicked() {
        btnClient.gameObject.SetActive(false);
        btnHost.gameObject.SetActive(false);
        txtStatus.text = "Waiting on Host";
        NetworkManager.Singleton.StartClient();
        btnClient.GetComponentInChildren<TextMeshProUGUI>().text = "Player Already Connected";
        btnClient.onClick.RemoveAllListeners();
    }
}
