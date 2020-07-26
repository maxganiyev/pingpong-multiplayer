using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public GameObject playBtn;
    public Text feedbackText;
    bool isConnecting;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Connect()
    {
        isConnecting = true;
        playBtn.SetActive(false);
        
        if (PhotonNetwork.IsConnected)
        {
            feedbackText.text = "Joining Room...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            feedbackText.text = "Connecting...";

            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = "1";
        }
    }

    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            feedbackText.text = "Finding room...";

            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        feedbackText.text = "Creating room...";

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        feedbackText.text = "<Color=Red>Disconnected: </Color> " + cause;
        
        isConnecting = false;
        playBtn.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        feedbackText.text = "<Color=Green>Joined room</Color>";

        PhotonNetwork.LoadLevel("MultiplayerScene");
    }
}
