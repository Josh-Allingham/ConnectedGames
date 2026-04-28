using Unity.VisualScripting;
using UnityEngine;


public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject exitMenu;
    public GameObject lobbyMenu;
    public void play()
    {
        mainMenu.SetActive(false);
        lobbyMenu.SetActive(true);
    }

    public void settings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void questionExit()
    {
        exitMenu.SetActive(true);
    }

    public void confirmExit()
    {
        exitMenu.SetActive(false);
        exit();
    }

    public void cancelExit()
    {
        exitMenu.SetActive(false);
    }

    public void exit()
    {
        Application.Quit();
    }


}
