using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class BattleNetManager : MonoBehaviourPunCallbacks
{
    string playerName = "P1";
    string gameVersion = "0.1";

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
    public GameObject disconnectScreen;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
        playerName = WorldToBattleTransfer.playerName;
        addPlayer(WorldToBattleTransfer.element);
    }

    private void Update()
    {
        //If you are host and players are spawned in, load the cpus needed to fill the rest of the scene
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

                    if ((timer >= 8) && !cpuLoaded)
                    {
                        int numCPU = 5 - PhotonNetwork.CurrentRoom.PlayerCount;
                        loadCPU(numCPU);
                    }
                }
            }
        }

        //Loads the battle menu for all
        if(cpuLoaded)
        {
            photonView.RPC("loadMenu", RpcTarget.AllBuffered);
        }

    }

    //If the master client switches, this means the host disconnected.
    //The way the scene is set up, the cpus will be destroyed, so return non host players back to the main menu if host disconnects
    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        disconnectScreen.SetActive(true);
    }

    //Records in console when the room is left
    public override void OnLeftRoom()
    {
        Debug.Log("Room Left!");
    }

    //Records in console reason for disconnection
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected: " + cause.ToString());
    }

    //Adds the battleplayer to the scene with the correct element and name
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

    //Adds the battleplayer to the player list for all players
    [PunRPC]
    public void addPlayerToList(string playerName)
    {
        playerNames.Add(playerName);
    }

    //Adds the element to the element list for all players
    [PunRPC]
    public void addElementToList(string element)
    {
        elements.Add(element);
    }

    //Loads the cpu to the scene and adds the cpu names and elements to the respective lists
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
            else if (!elements.Contains("Chaos"))
            {
                elementsNeeded[e] = "Chaos";
                photonView.RPC("addElementToList", RpcTarget.AllBuffered, elementsNeeded[e]);
            }
        }

        for (int i = 0; i < cpuNeeded; i++)
        {
            GameObject newCPU = PhotonNetwork.Instantiate(cpuPrefab.name, new Vector3(0, 1, 0), Quaternion.identity, 0);
            newCPU.transform.parent = myPlayerManager.transform;
            newCPU.GetComponent<BattleCPU>().playerName = "CPU " + (i + 1);
            newCPU.GetComponent<BattleCPU>().playerElement = elementsNeeded[i];
            photonView.RPC("addPlayerToList", RpcTarget.AllBuffered, newCPU.GetComponent<BattlePlayer>().playerName);
        }
        cpuLoaded = true;

    }

    //Loads the battle menu for all players
    [PunRPC]
    public void loadMenu()
    {
        battleMenu.SetActive(true);
    }

    //Leaves the room
    public void leaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
