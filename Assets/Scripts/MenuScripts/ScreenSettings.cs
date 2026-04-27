using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSettings : MonoBehaviour
{
    [SerializeField]
    private Button fullscreenBtn;
    [SerializeField]
    private Button windowedBtn;
    [SerializeField]
    private Button borderlessBtn;

    private void Start()
    {
        if(PlayerPrefs.HasKey("WindowType"))
        {
            loadScreenSettings();
        }
        else
        {
            changeWindowType("Fullscreen");
        }
    }

    public void changeWindowType(string type)
    {
        StartCoroutine(switchWindow(type));
    }

    IEnumerator switchWindow(string type)
    {
        if (type == "Fullscreen")
        {
            fullscreenBtn.interactable = false;
            windowedBtn.interactable = true;
            borderlessBtn.interactable = true;
            Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
            PlayerPrefs.SetString("WindowType", type);
            yield return null;
        }
        else if (type == "Windowed")
        {
            fullscreenBtn.interactable = true;
            windowedBtn.interactable = false;
            borderlessBtn.interactable = true;
            Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
            PlayerPrefs.SetString("WindowType", type);
            yield return null;
        }
        else if (type == "Borderless")
        {
            fullscreenBtn.interactable = true;
            windowedBtn.interactable = true;
            borderlessBtn.interactable = false;
            Screen.SetResolution(1920, 1080, FullScreenMode.MaximizedWindow);
            PlayerPrefs.SetString("WindowType", type);
            yield return null;
        }
    }

    public void loadScreenSettings()
    {
        string windowType = PlayerPrefs.GetString("WindowType");
        changeWindowType(windowType);
    }
}
