using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Threading;
public class BattleNetManager : MonoBehaviourPunCallbacks
{
    string playerName = "P1";
    string playerElement = "0";
    string gameVersion = "0.1";
    List<RoomInfo> createdRooms = new List<RoomInfo>();
    string roomName = "Room 1";
    int maxPlayers = 4;
    Vector2 roomListScroll = Vector2.zero;
    bool joiningRoom = false;
    bool render = true;

    public GameObject playerPrefab;
    public GameObject cpuPrefab;
    public List<GameObject> players = new List<GameObject>();
    public List<string> elements = new List<string>();

    public int expectedPlayerCount;
    public bool timeStarted = false;
    public float timeToConnect = 5f;
    public float timerCurrent;
    public bool cpuLoaded = false;

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
        if (!timeStarted && PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            RPCTimerStart();
        }
    }

    private void FixedUpdate()
    {
        if(timeStarted == true)
        {
            timeToConnect = RPCCheckTimer();
            if (timeToConnect <= 0f && !cpuLoaded)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                loadCPU();
            }
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
        AddPlayer(int.Parse(playerElement));
    }

    void AddPlayer(int element = 0)
    {
        //spawn player
        GameObject newPlayer = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 1, 0), Quaternion.identity, 0);
        newPlayer.GetComponent<BattlePlayer>().playerName = playerName;

        switch (element)
        {
            case 0:
                newPlayer.GetComponent<BattlePlayer>().playerElement = "Water";
                break;
            case 1:
                newPlayer.GetComponent<BattlePlayer>().playerElement = "Fire";
                break;
            case 2:
                newPlayer.GetComponent<BattlePlayer>().playerElement = "Earth";
                break;
            case 3:
                newPlayer.GetComponent<BattlePlayer>().playerElement = "Wind";
                break;
        }
        players.Add(newPlayer);
        elements.Add(newPlayer.GetComponent<BattlePlayer>().playerElement);
    }

    [PunRPC]
    void RPCTimerStart()
    {
        timerCurrent = timeToConnect;
        timeStarted = true;
        joinTimerTick();
    }

    [PunRPC]
    void joinTimerTick()
    {
        timerCurrent -= Time.deltaTime;
    }

    [PunRPC]
    float RPCCheckTimer()
    {
        return timerCurrent;
    }

    void loadCPU()
    {
        int cpuNeeded = 4 - PhotonNetwork.CurrentRoom.PlayerCount;
        string[] elementsNeeded = new string[cpuNeeded];

        for (int e = 0; e < elementsNeeded.Length; e++)
        {
            if (!elements.Contains("Water"))
            {
                elementsNeeded[e] = "Water";
                break;
            }
            else if (!elements.Contains("Fire"))
            {
                elementsNeeded[e] = "Fire";
                break;
            }
            else if (!elements.Contains("Earth"))
            {
                elementsNeeded[e] = "Earth";
                break;
            }
            else if (!elements.Contains("Wind"))
            {
                elementsNeeded[e] = "Wind";
                break;
            }
        }

        for (int i = 0; i < cpuNeeded; i++)
        {
            GameObject newCPU = PhotonNetwork.Instantiate(cpuPrefab.name, new Vector3(0, 1, 0), Quaternion.identity, 0);
            newCPU.GetComponent<BattleCPU>().playerName = "CPU " + (i + 1);
            newCPU.GetComponent<BattleCPU>().playerElement = elementsNeeded[i];
            players.Add(newCPU);
        }
        cpuLoaded = true;
    }
}
