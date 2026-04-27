using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [SerializeField]
    private Button hintsOn;
    [SerializeField]
    private Button hintsOff;
    [SerializeField]
    private Button subtitlesOn;
    [SerializeField]
    private Button subtitleOff;

    private bool hintsEnabled;
    private bool subtitlesEnabled;

    private void Start()
    {
        if (PlayerPrefs.HasKey("HintsEnabled") && PlayerPrefs.HasKey("SubtitlesEnabled"))
        {
            loadGameSettings();
        }
        else
        {
            enableHints();
            enableSubtitles();
        }
    }

    public void enableHints()
    {
        hintsEnabled = true;
        hintsOn.interactable = false;
        hintsOff.interactable = true;
        PlayerPrefs.SetInt("HintsEnabled", (hintsEnabled ? 1:0)); 
    }

    public void disableHints()
    {
        hintsEnabled = false;
        hintsOn.interactable = true;
        hintsOff.interactable = false;
        PlayerPrefs.SetInt("HintsEnabled", (hintsEnabled ? 1 : 0));
    }

    public void enableSubtitles()
    {
        subtitlesEnabled = true;
        subtitlesOn.interactable = false;
        subtitleOff.interactable = true;
        PlayerPrefs.SetInt("SubtitlesEnabled", (subtitlesEnabled ? 1:0)); 
    }

    public void disableSubtitles()
    {
        subtitlesEnabled = false;
        subtitlesOn.interactable = true;
        subtitleOff.interactable = false;
        PlayerPrefs.SetInt("SubtitlesEnabled", (subtitlesEnabled ? 1 : 0));
    }

    public void loadGameSettings()
    {
        if(PlayerPrefs.GetInt("HintsEnabled") == 1)
        {
            enableHints();
        }
        else
        {
            disableHints();
        }

        if (PlayerPrefs.GetInt("SubtitlesEnabled") == 1)
        {
            enableSubtitles();
        }
        else
        {
            disableSubtitles();
        }
    }

}
