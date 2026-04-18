using Unity.VisualScripting;
using UnityEngine;


public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject exitMenu;

    public void play()
    {

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
