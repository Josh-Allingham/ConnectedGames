using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyRoomScript : MonoBehaviourPunCallbacks
{
    [Header("Lobby Room Player Info")]
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;
    public string player1Name;
    public string player2Name;
    public string player3Name;
    public string player4Name;

    [Header("Lobby Room Objects")]
    public TMP_Text playerCount;
    public Button startGameBtn;
    public GameObject startGameBtnImage;
    public TMP_Text readyBtnText;
    public GameObject player1Profile;
    public GameObject player2Profile;
    public GameObject player3Profile;
    public GameObject player4Profile;
    public GameObject playerProfiles;

    public TMP_Text lobbyRoomName;

    public string[] elements = new string[] { "Water", "Fire", "Earth", "Wind" };

    public bool arrowsSpawned = false;

    public void Update()
    {
        photonView.RPC("RPCUpdatePlayerCount", RpcTarget.AllBuffered);

    }


    [PunRPC]
    public void RPCHostJoin(string chosenName, string roomName)
    {
        PhotonNetwork.Instantiate(, , Quaternion.identity, 0);
        player1Name = chosenName;
        lobbyRoomName.text = roomName;
        if(photonView.IsMine)
        {
            photonView.RPC("RPCHostJoin", RpcTarget.OthersBuffered, chosenName, roomName);
        }
    }

    [PunRPC]
    public void RPCUpdatePlayerCount()
    {
        playerCount.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
    }


    public void rightArrowElem()
    {

    }

    public void leftArrowElem()
    {

    }
}
