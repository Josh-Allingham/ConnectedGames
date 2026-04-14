using Unity.VisualScripting;
using UnityEngine;


public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;

    public void play()
    {

    }

    public void settings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void exit()
    {
        Application.Quit();
    }
}
