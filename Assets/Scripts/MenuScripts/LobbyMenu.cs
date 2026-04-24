using UnityEngine;

public class LobbyMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject lobbyMenu;
    public GameObject joinCreateMenu;
    public GameObject joinMenu;
    public GameObject createRoomMenu;
    public GameObject lobbyRoom;

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


    public void openJoinRoom()
    {
        joinCreateMenu.SetActive(false);
        joinMenu.SetActive(true);
    }


    public void createRoom()
    {

    }

    public void joinRoom()
    {

    }
}
