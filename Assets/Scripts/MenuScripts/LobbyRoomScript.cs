using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class LobbyRoomScript : MonoBehaviourPunCallbacks
{
    [Header("Lobby Room Player Info")]
    public GameObject playerProfile1;
    public GameObject playerProfile2;
    public GameObject playerProfile3;
    public GameObject playerProfile4;
    public List<GameObject> allProfiles = new List<GameObject>();

    [Header("Lobby Room Objects")]
    public TMP_Text playerCount;
    public Button startGameBtn;
    public GameObject startGameBtnImage;
    public TMP_Text readyBtnText;

    public TMP_Text lobbyRoomName;

    public string[] elementsAvailable = new string[] { "Water", "Fire", "Earth", "Wind" };
    public string[] elementsTaken = new string[4];

    public GameObject myProfile;

    public void Update()
    {
        if(PhotonNetwork.CurrentRoom != null)
        {
            playerCount.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        }
        

    }

    public void hostJoin(string chosenName, string roomName)
    {
        lobbyRoomName.text = roomName;
        instantiatePlayer(chosenName, 1);

    }

    public void playerJoin(string chosenName)
    {
        int player = PhotonNetwork.CurrentRoom.PlayerCount;
        instantiatePlayer(chosenName, player);

    }

    public void instantiatePlayer(string playerName, int playerNum)
    {
        Debug.Log(playerName);
        switch (playerNum)
        {
            case 1:
                playerProfile1.transform.GetChild(1).gameObject.SetActive(true);
                myProfile = playerProfile1;
                photonView.RPC("RPCAnnounceProfile", RpcTarget.AllBuffered, 1, playerName);
                break;
            case 2:
                playerProfile2.transform.GetChild(1).gameObject.SetActive(true);
                myProfile = playerProfile2;
                photonView.RPC("RPCAnnounceProfile", RpcTarget.AllBuffered, 2, playerName);
                break;
            case 3:
                playerProfile3.transform.GetChild(1).gameObject.SetActive(true);
                photonView.RPC("RPCAnnounceProfile", RpcTarget.AllBuffered, 3, playerName);
                myProfile = playerProfile3;
                break;
            case 4:
                playerProfile4.transform.GetChild(1).gameObject.SetActive(true);
                photonView.RPC("RPCAnnounceProfile", RpcTarget.AllBuffered, 4, playerName);
                myProfile = playerProfile4;
                break;
        }
    }


    public void rightArrowElem()
    {
        string currentElem = myProfile.GetComponent<PlayerProfile>().elementSelected;
        string newElem = currentElem;
        for (int i = 0; i < elementsAvailable.Length; i++)
        {
            if(currentElem == elementsAvailable[i])
            {
                if(i+1 < elementsAvailable.Length)
                {
                    newElem = elementsAvailable[i + 1];
                }
                else
                {
                    newElem = elementsAvailable[0];
                }
            }
        }

        myProfile.GetComponent<PlayerProfile>().elementSelected = newElem;
    }

    public void leftArrowElem()
    {
        string currentElem = myProfile.GetComponent<PlayerProfile>().elementSelected;
        string newElem = currentElem;
        for (int i = 0; i < elementsAvailable.Length; i++)
        {
            if (currentElem == elementsAvailable[i])
            {
                if (i - 1 > 0)
                {
                    newElem = elementsAvailable[i - 1];
                }
                else
                {
                    newElem = elementsAvailable[elementsAvailable.Length-1];
                }
            }
        }

        myProfile.GetComponent<PlayerProfile>().elementSelected = newElem;
    }

    [PunRPC]
    public void RPCAnnounceProfile(int profile, string name)
    {
        allProfiles.Add(playerProfile1);
        allProfiles.Add(playerProfile2);
        allProfiles.Add(playerProfile3);
        allProfiles.Add(playerProfile4);

        for (int i = 0; i < 4; i++)
        {
            if (profile - 1 == i)
            {
                allProfiles[i].SetActive(true);
                allProfiles[i].GetComponent<PlayerProfile>().profileName = name;
                allProfiles[i].GetComponent<PlayerProfile>().elementSelected = elementsAvailable[0];
                
            }
        }

        allProfiles.Clear();
    }


    [PunRPC]
    public void changeElement()
    {

    }

}
