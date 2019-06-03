using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks {

    public static PhotonLobby lobby;

    public GameObject battleButton;
    public GameObject cancelButton;

    private void Awake()
    {
        lobby = this; //Creates the singleton, lives within the Main Menu Scene
    }

    // Use this for initialization
    void Start () {
        PhotonNetwork.ConnectUsingSettings();
	}

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the Photon master server.");
        PhotonNetwork.AutomaticallySyncScene = true;
        battleButton.SetActive(true);

        //Trigger join automatically
        OnBattleButtonClicked();
    }

    public void OnBattleButtonClicked()
    {
        Debug.Log("Battle Button was clicked.");
        PhotonNetwork.JoinRandomRoom();
        battleButton.SetActive(false);
        cancelButton.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a random game but failed. There must be no open games available.");
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("Trying to create a new room...");
        int randomRoomName = Random.Range(0, 1000);
        RoomOptions roomOps = new RoomOptions() {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = (byte) MultiplayerSettings.multiplayerSettings.maxPlayers
        };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed, there must already be a room with the same name.");
        CreateRoom();
    }

    public void OnCancelButtonClicked()
    {
        Debug.Log("CancelButton clicked.");
        cancelButton.SetActive(false);
        battleButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}
