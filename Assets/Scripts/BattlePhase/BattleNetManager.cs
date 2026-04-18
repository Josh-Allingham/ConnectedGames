using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
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

    public GameObject myPlayerManager;
    public GameObject playerPrefab;
    public GameObject cpuPrefab;
    public List<string> playerNames = new List<string>();
    public List<string> elements = new List<string>();

    public int expectedPlayerCount;
    public bool cpuLoaded = false;

    public bool timerStarted = false;
    public double start = 0;
    public double timer = 0;

    public GameObject battleMenu;

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
        if(PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom != null)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount > 0)
                {
                    if (timerStarted == false)
                    {
                        start = PhotonNetwork.Time;
                        timerStarted = true;
                    }
    
                    if (timerStarted)
                    {
                        timer = PhotonNetwork.Time - start;
                    }

                    if (timer >= 8 && !cpuLoaded)
                    {
                        int numCPU = 4 - PhotonNetwork.CurrentRoom.PlayerCount;
                        loadCPU(numCPU);
                    }
                }
            }
        }

        if(cpuLoaded)
        {
            photonView.RPC("loadMenu", RpcTarget.AllBuffered);
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
        addPlayer(int.Parse(playerElement));
    }

    public void addPlayer(int element = 0)
    {
        //spawn player
        Vector3 spawnPoint = new Vector3(0, 1, 0);
        GameObject newPlayer = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint, Quaternion.identity, 0);
        newPlayer.transform.parent = myPlayerManager.transform;
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
        photonView.RPC("addPlayerToList", RpcTarget.AllBuffered, newPlayer.GetComponent<BattlePlayer>().playerName);
        photonView.RPC("addElementToList", RpcTarget.AllBuffered, newPlayer.GetComponent<BattlePlayer>().playerElement);

    }

    [PunRPC]
    public void addPlayerToList(string playerName)
    {
        playerNames.Add(playerName);
    }

    [PunRPC]
    public void addElementToList(string element)
    {
        elements.Add(element);
    }


    void loadCPU(int cpuNeeded)
    {
        string[] elementsNeeded = new string[cpuNeeded];

        for (int e = 0; e < elementsNeeded.Length; e++)
        {
            if (!elements.Contains("Water"))
            {
                elementsNeeded[e] = "Water";
                photonView.RPC("addElementToList", RpcTarget.AllBuffered, elementsNeeded[e]);
            }
            else if (!elements.Contains("Fire"))
            {
                elementsNeeded[e] = "Fire";
                photonView.RPC("addElementToList", RpcTarget.AllBuffered, elementsNeeded[e]);
            }
            else if (!elements.Contains("Earth"))
            {
                elementsNeeded[e] = "Earth";
                photonView.RPC("addElementToList", RpcTarget.AllBuffered, elementsNeeded[e]);
            }
            else if (!elements.Contains("Wind"))
            {
                elementsNeeded[e] = "Wind";
                photonView.RPC("addElementToList", RpcTarget.AllBuffered, elementsNeeded[e]);
            }
        }

        for (int i = 0; i < cpuNeeded; i++)
        {
            GameObject newCPU = PhotonNetwork.Instantiate(cpuPrefab.name, new Vector3(0, 1, 0), Quaternion.identity, 0);
            newCPU.GetComponent<BattleCPU>().playerName = "CPU " + (i + 1);
            newCPU.GetComponent<BattleCPU>().playerElement = elementsNeeded[i];
            photonView.RPC("addPlayerToList", RpcTarget.AllBuffered, newCPU.GetComponent<BattlePlayer>().playerName);
        }
        cpuLoaded = true;

    }

    [PunRPC]
    public void loadMenu()
    {
        battleMenu.SetActive(true);
    }

}
