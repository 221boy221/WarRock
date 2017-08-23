using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager Instance;

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        DontDestroyOnLoad(this);

        //
        ServerManager.Instance.OnConnectedToServer += OnConnectedToServer;
    }

    private void OnConnectedToServer() {
        ServerManager.Instance.OnConnectedToServer -= OnConnectedToServer;

        Debug.Log("Moving client towards the Lobby.");

        // Load Lobby
        PhotonNetwork.JoinLobby();
        PhotonNetwork.LoadLevel("Lobby");
        PhotonNetwork.automaticallySyncScene = true;
    }
}
