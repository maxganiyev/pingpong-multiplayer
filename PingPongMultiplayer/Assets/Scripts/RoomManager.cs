using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public GameObject loaderObj;

    public Text upScoreText;
    public Text downScoreText;

    public int upScore;
    public int downScore;

    public GameObject playerPrefab;
    public GameObject ballPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Lobby");
            return;
        }

        if (Player.LocalPlayerInstance == null)
        {
            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, -4.5f, 0f), Quaternion.identity, 0);
        }
    }

    private void Update()
    {
        upScoreText.text = upScore.ToString();
        downScoreText.text = downScore.ToString();
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            loaderObj.SetActive(false);
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                PhotonNetwork.Instantiate(this.ballPrefab.name, Vector3.zero, Quaternion.identity, 0);
                loaderObj.SetActive(false);
                GameObject.FindGameObjectsWithTag("Player")[0].transform.position = new Vector3(0, 4.5f, 0);
            }
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player other)
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
