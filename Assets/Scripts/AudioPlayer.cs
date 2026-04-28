using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer main;
    public AudioSource camSource;
    void Start()
    {
        main = this;   
    }

    public void PlaySound(AudioClip audio)
    {
        camSource.PlayOneShot(audio);
    }
}
