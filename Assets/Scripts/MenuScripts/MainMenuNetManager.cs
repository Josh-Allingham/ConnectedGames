using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MainMenuNetManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    public TMP_InputField createRoomName;
    [SerializeField]
    public TMP_InputField createPlayerName;
    [SerializeField]
    public TMP_InputField setPassword;

    [SerializeField]
    public TMP_InputField joinRoomName;
    [SerializeField]
    public TMP_InputField joinPlayerName;
    [SerializeField]
    public TMP_InputField joinUsingPassword;

    public string gameVersion = "0.1";
    public bool isPrivate;
    public int maxPlayers = 4;
    public bool joiningRoom = false;

    List<RoomInfo> createdRooms = new List<RoomInfo>();
    Vector2 roomListScroll = Vector2.zero;
    bool render = true;

    public LobbyMenu lobbyMenu;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }


    }

    private void Update()
    {
        if(setPassword.interactable == false)
        {
            isPrivate = false;
        }
        else
        {
            isPrivate = true;
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Room Left!");
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected: " + cause.ToString());
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connection made to " + PhotonNetwork.CloudRegion + " server.");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }

    public override void OnJoinedRoom()
    {
        string correctPassword = (string)PhotonNetwork.CurrentRoom.CustomProperties["Password"];

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("Connected to Room");
            lobbyMenu.createRoom();
        }
        else if (joinUsingPassword.text == correctPassword)
        {
            Debug.Log("Connected to Room");
            print(PhotonNetwork.CurrentRoom.Players.Count);
            lobbyMenu.joinRoom();
        }
        else if(PhotonNetwork.CurrentRoom.PlayerCount == 4)
        {
            PhotonNetwork.LeaveRoom();
            lobbyMenu.toPreviousMenu("Lobby Room");
        }
        else
        {
            PhotonNetwork.LeaveRoom();
            lobbyMenu.toPreviousMenu("Lobby Room");
        }

    }

public void createRoom()
    {
        Debug.Log("Called create");
        if (createRoomName.text != "")
        {
            joiningRoom = true;
            RoomOptions roomOptions = new RoomOptions();
            if (setPassword.text !="")
            {
                Hashtable customProps = new Hashtable();
                customProps.Add("Password", setPassword.text);
                roomOptions.CustomRoomProperties = customProps;
            }

            roomOptions.IsOpen = true;
            roomOptions.IsVisible = true;
            roomOptions.MaxPlayers = (byte)maxPlayers;

            PhotonNetwork.JoinOrCreateRoom(createRoomName.text, roomOptions, TypedLobby.Default);
            Debug.Log("Room created " + createRoomName.text);
        }
    }


    public void joinPasswordRoom()
    {
        joiningRoom = true;
        PhotonNetwork.NickName = joinPlayerName.text;
        PhotonNetwork.JoinRoom(joinRoomName.text);
    }

    public void leaveRoom()
    {
        lobbyMenu.toPreviousMenu("Lobby Room");
        PhotonNetwork.LeaveRoom();
    }
}
