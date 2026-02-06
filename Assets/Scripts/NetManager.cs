using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
public class NetManager : MonoBehaviourPunCallbacks
{
    string playerName = "P1";
    string playerElement = "0";
    string gameVersion = "0.1";
    List<RoomInfo> createdRooms = new List<RoomInfo> ();
    string roomName = "Room 1";
    int maxPlayers = 4;
    Vector2 roomListScroll = Vector2.zero;
    bool joiningRoom = false;
    bool render = true;

    public GameObject playerPrefab;
    public List<GameObject> players = new List<GameObject> ();

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Rooms received");
        createdRooms = roomList;
    }

    private void OnGUI()
    {
        if (render)
        {
            GUI.Window(0, new Rect(Screen.width / 2 - 450, Screen.height / 2 - 200, 900, 400), LobbyWindow, "Lobby");
        }
    }

    void LobbyWindow(int index)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Status: " + PhotonNetwork.NetworkClientState);

        if (joiningRoom || !PhotonNetwork.IsConnected || PhotonNetwork.NetworkClientState != ClientState.JoinedLobby)
        {
            GUI.enabled = false;
        }

        GUILayout.FlexibleSpace();

        roomName = GUILayout.TextField(roomName, GUILayout.Width(250));

        if (GUILayout.Button("Create Room", GUILayout.Width(125)))
        {
            if (roomName != "")
            {
                joiningRoom = true;

                RoomOptions roomOptions = new RoomOptions();
                roomOptions.IsOpen = true;
                roomOptions.IsVisible = true;
                roomOptions.MaxPlayers = (byte)maxPlayers;

                PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
            }
        }

        GUILayout.EndHorizontal();

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
                    PhotonNetwork.NickName = playerName;
                    PhotonNetwork.JoinRoom(createdRooms[i].Name);
                }
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.EndScrollView();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Player Name: ", GUILayout.Width(85));
        playerName = GUILayout.TextField(playerName, GUILayout.Width(250));
        GUILayout.Label("Element: ", GUILayout.Width(85));
        playerElement = GUILayout.TextField(playerElement, GUILayout.Width(50));

        string elementChosen = "";
        switch (playerElement)
        {
            case "0":
                elementChosen = "Water";
                break;
            case "1":
                elementChosen = "Fire";
                break;
            case "2":
                elementChosen = "Earth";
                break;
            case "3":
                elementChosen = "Wind";
                break;
        }

        GUILayout.Label(elementChosen, GUILayout.Width(85));

        GUILayout.FlexibleSpace();
        
        GUI.enabled = (PhotonNetwork.NetworkClientState == ClientState.JoinedLobby || PhotonNetwork.NetworkClientState == ClientState.Disconnected) && !joiningRoom;
        if (GUILayout.Button("Refresh", GUILayout.Width(100)))
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinLobby(TypedLobby.Default);
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        GUILayout.EndHorizontal();

        if (joiningRoom)
        {
            GUI.enabled = true;
            GUI.Label(new Rect(900 / 2 - 50, 400 / 2 - 10, 100, 20), "Connecting...");
        }

    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Connected to Room");
        print(PhotonNetwork.CurrentRoom.Players.Count);
        render = false;
        AddPlayer(int.Parse(playerElement), true);
        
    }

    void AddPlayer(int element = 0, bool setCameraTarget = false)
    {
        //spawn player
        GameObject newPlayer = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 1, 0), Quaternion.identity, 0);

        switch (playerElement)
        {
            case "0":
                newPlayer.GetComponent<Player>().currentType = Player.PlayerType.Water;
                break;
            case "1":
                newPlayer.GetComponent<Player>().currentType = Player.PlayerType.Fire;
                break;
            case "2":
                newPlayer.GetComponent<Player>().currentType = Player.PlayerType.Earth;
                break;
            case "3":
                newPlayer.GetComponent<Player>().currentType = Player.PlayerType.Wind;
                break;
        }
        
        players.Add(newPlayer);
        if (setCameraTarget)
        {
            CameraController.main.SetTarget(newPlayer.transform);
        }
    }
}
