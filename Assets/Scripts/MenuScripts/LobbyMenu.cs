using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LobbyMenu : MonoBehaviour
{
    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject lobbyMenu;
    public GameObject joinCreateMenu;
    public GameObject joinMenu;
    public GameObject createRoomMenu;
    public GameObject lobbyRoom;

    [Header("Inputs")]
    public TMP_InputField createRoomName;
    public TMP_InputField makePassword;
    public TMP_InputField joinRoomName;
    public TMP_InputField joinPassword;

    [Header("Objects")]
    public GameObject lockImg;
    public GameObject unlockImg;

    public void toPreviousMenu(string currentMenu)
    {
        
        if(currentMenu == "Join/Create Menu")
        {
            lobbyMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
        else if(currentMenu == "Create Menu")
        {
            createRoomMenu.SetActive(false);
            joinCreateMenu.SetActive(true);
        }
        else if(currentMenu == "Join Menu")
        {
            joinMenu.SetActive(false);
            joinCreateMenu.SetActive(true);
        }
        else if(currentMenu == "Lobby Room")
        {
            lobbyRoom.SetActive(false);
            joinCreateMenu.SetActive(true);
            lobbyMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }

    public void openCreateRoom()
    {
        joinCreateMenu.SetActive(false);
        createRoomMenu.SetActive(true);
    }

    public void makePrivate()
    {
        unlockImg.SetActive(false);
        lockImg.SetActive(true);
        makePassword.interactable = true;
    }

    public void makePublic()
    {
        lockImg.SetActive(false);
        unlockImg.SetActive(true);
        makePassword.interactable = false;
    }

    public void openJoinRoom()
    {
        joinCreateMenu.SetActive(false);
        joinMenu.SetActive(true);
    }


    public void createRoom()
    {
        createRoomMenu.SetActive(false);
        lobbyRoom.SetActive(true);
    }

    public void joinRoom()
    {

    }


}
