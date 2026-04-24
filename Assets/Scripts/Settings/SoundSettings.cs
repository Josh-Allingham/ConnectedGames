using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField]
    private AudioMixer masterMixer;
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;

    private float masterVolume;
    private float sfxVolume;
    private float musicVolume;


    public void Start()
    {
        if(PlayerPrefs.HasKey("MasterVolumeLevel") && PlayerPrefs.HasKey("MusicVolumeLevel") && PlayerPrefs.HasKey("SFXVolumeLevel"))
        {
            loadVolume();
        }
        else
        {
            setMasterVolume();
            setSFXVolume();
            setMusicVolume();
        }
    }

    private void Update()
    {

    }

    public void setMasterVolume()
    {
        masterVolume = masterSlider.value;
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 20);
        PlayerPrefs.SetFloat("MasterVolumeLevel", masterVolume);
    }

    public void setSFXVolume()
    {
        sfxVolume = sfxSlider.value;
        masterMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
        PlayerPrefs.SetFloat("SFXVolumeLevel", sfxVolume);
    }

    public void setMusicVolume()
    {
        musicVolume = musicSlider.value;
        masterMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        PlayerPrefs.SetFloat("MusicVolumeLevel", musicVolume);
    }

    public void loadVolume()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolumeLevel");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolumeLevel");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolumeLevel");

        setMasterVolume();
        setMusicVolume();
        setSFXVolume();
    }

}