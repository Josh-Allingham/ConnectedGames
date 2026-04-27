using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuNetManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    public TMP_InputField roomName;
    [SerializeField]
    public TMP_InputField playerName;
    [SerializeField]
    public TMP_InputField setPassword;

    public string gameVersion = "0.1";
    public bool isPrivate;
    public int maxPlayers = 4;
    public bool joiningRoom = false;

    List<RoomInfo> createdRooms = new List<RoomInfo>();
    Vector2 roomListScroll = Vector2.zero;
    bool render = true;



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
        Debug.Log("Connected to Room");
        print(PhotonNetwork.CurrentRoom.Players.Count);
    }

    public void createLobby()
    {
        if (roomName.text != "")
        {
            joiningRoom = true;

            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsOpen = true;
            roomOptions.IsVisible = true;
            roomOptions.MaxPlayers = (byte)maxPlayers;

            PhotonNetwork.JoinOrCreateRoom(roomName.text, roomOptions, TypedLobby.Default); 
        }
    }


    public void joinLobby()
    {
        roomListScroll = GUILayout.BeginScrollView(roomListScroll, true, true);

        if (createdRooms.Count == 0)
        {
            GUILayout.Label("No Rooms exist.");
        }
        else
        {
            for (int i = 0; i < createdRooms.Count; i++)
            {
                GUILayout.BeginHorizontal("box");
                GUILayout.Label(createdRooms[i].Name, GUILayout.Width(400));
                GUILayout.Label(createdRooms[i].PlayerCount + "/" + createdRooms[i].MaxPlayers);

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Join Room"))
                {
                    joiningRoom = true;
                    PhotonNetwork.NickName = playerName.text;
                    PhotonNetwork.JoinRoom(createdRooms[i].Name);
                }
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.EndScrollView();        
    }
}
