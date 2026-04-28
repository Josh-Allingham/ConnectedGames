using UnityEngine;

public class BattleSounds : MonoBehaviour
{
    [Header("Source")]
    public AudioSource source;

    [Header("Sounds")]
    public AudioClip buttonSound;


    public void clickButton()
    {
        source.PlayOneShot(buttonSound);
    }
}
