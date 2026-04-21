using UnityEngine;

public class LobbyMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject lobbyMenu;
    public GameObject joinMenu;
    public GameObject createRoomMenu;
    public GameObject lobbyRoom;



    public void createRoom()
    {
        lobbyMenu.SetActive(false);
        createRoomMenu.SetActive(true);
    }


    public void joinRoom()
    {
        lobbyMenu.SetActive(false);
        joinMenu.SetActive(true);
    }


    public void backToMainMenu()
    {
        lobbyMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
