using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer main;
    public AudioSource camSource;
    [SerializeField] private AudioClip buttonSound;
    void Start()
    {
        main = this;   
    }

    public void PlaySound(AudioClip audio)
    {
        camSource.PlayOneShot(audio);
    }

    public void ButtonSound()
    {
        camSource.PlayOneShot(buttonSound);
    }
}
