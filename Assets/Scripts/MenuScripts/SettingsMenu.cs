using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject settingsSelectMenu;
    public GameObject screenSettingsMenu;
    public GameObject soundSettingsMenu;
    public GameObject gameSettingsMenu;

    public void screenSettings()
    {
        settingsSelectMenu.SetActive(false);
        screenSettingsMenu.SetActive(true);
    }

    public void soundSettings()
    {
        settingsSelectMenu.SetActive(false);
        soundSettingsMenu.SetActive(true);
    }

    public void gameSettings()
    {
        settingsSelectMenu.SetActive(false);
        gameSettingsMenu.SetActive(true);
    }

    public void backToSettingsMenu()
    {
        screenSettingsMenu.SetActive(false);
        soundSettingsMenu.SetActive(false);
        gameSettingsMenu.SetActive(false);
        settingsSelectMenu.SetActive(true);
    }

    public void backToMainMenu()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

}
