using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LobbyRoomScript : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
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
    public int myNumProfile;

    public void Start()
    {
        photonView.OwnershipTransfer = OwnershipOption.Takeover;
    }

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
                myNumProfile = 1;
                photonView.RPC("RPCAnnounceProfile", RpcTarget.AllBuffered, 1, "(HOST) " + playerName);
                break;
            case 2:
                playerProfile2.transform.GetChild(1).gameObject.SetActive(true);
                myProfile = playerProfile2;
                var id = playerProfile2.GetComponent<PhotonView>().ViewID;
                PhotonView view = PhotonView.Find(id);
                view.TransferOwnership(PhotonNetwork.LocalPlayer);
                myNumProfile = 2;
                photonView.RPC("RPCAnnounceProfile", RpcTarget.AllBuffered, 2, playerName);
                break;
            case 3:
                playerProfile3.transform.GetChild(1).gameObject.SetActive(true);
                myProfile = playerProfile3;
                playerProfile3.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
                myNumProfile = 3;
                photonView.RPC("RPCAnnounceProfile", RpcTarget.AllBuffered, 3, playerName);
                
                break;
            case 4:
                playerProfile4.transform.GetChild(1).gameObject.SetActive(true);
                myProfile = playerProfile4;
                playerProfile4.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
                myNumProfile = 4;
                photonView.RPC("RPCAnnounceProfile", RpcTarget.AllBuffered, 4, playerName);
                
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
        Debug.Log(newElem);
        myProfile.GetComponent<PlayerProfile>().updateProfile(myProfile.GetComponent<PlayerProfile>().profileName, newElem);
        //myProfile.GetComponent<PlayerProfile>().elementSelected = newElem;
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
                else if(i - 1 == 0)
                {
                    newElem = elementsAvailable[0];
                }
                else
                {
                    newElem = elementsAvailable[elementsAvailable.Length - 1];
                }
            }
        }
        Debug.Log(newElem);
        myProfile.GetComponent<PlayerProfile>().updateProfile(myProfile.GetComponent<PlayerProfile>().profileName, newElem);
        //myProfile.GetComponent<PlayerProfile>().elementSelected = newElem;
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

    public void OnOwnershipRequest(PhotonView targetView, Photon.Realtime.Player requestingPlayer)
    {
        Debug.Log(requestingPlayer);
        targetView.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Photon.Realtime.Player previousOwner)
    {
        Debug.Log("Transfer Successfull!");
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Photon.Realtime.Player senderOfFailedRequest)
    {
        Debug.Log("Transfer Failed!");
    }
}
