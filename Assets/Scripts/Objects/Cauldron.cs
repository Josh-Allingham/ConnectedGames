using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Cauldron : MonoBehaviour, IElementInteractable
{
    [SerializeField] private bool isLit = false, hasWater = false, hasWind = false, hasWood = false;
    [SerializeField] private GameObject wood;
    [SerializeField] private GameObject water; 
    [SerializeField] private Vector2 waterScaleMinMax = new Vector2(0, 0.2f); //scale y from 0 to .2
    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject steam;

    private AudioSource cauldronAudioSource;
    public AudioClip flameSound;
    public AudioClip earthSound;
    public AudioClip windSound;
    
    void Start()
    {
        cauldronAudioSource = GetComponent<AudioSource>();
        wood.transform.position += Vector3.down * 2;
        water.transform.localScale = new Vector3(water.transform.localScale.x, waterScaleMinMax.x, water.transform.localScale.z);
        fire.SetActive(false);
        steam.SetActive(false);
    }

    void Update()
    {
        if (isLit && hasWater && hasWood)
        {
            steam.SetActive(true);
        }
    }

    public bool IsActive()
    {
        return isLit && hasWater && hasWind && hasWood;
    }

    public void TouchWater()
    {
        if (!hasWater)
        {
            hasWater = true;
            StartCoroutine(AddWater(0.5f));
        }
        
    }

    private IEnumerator AddWater(float riseTime)
    {
        float count = 0;
        while (count < riseTime)
        {
            count += Time.deltaTime;
            water.transform.localScale = new Vector3(water.transform.localScale.x, waterScaleMinMax.y * Time.deltaTime / riseTime, water.transform.localScale.z);
        }
        yield return null;
    }
    public void TouchFire(bool isCharged)
    {
        if (!isLit && hasWood && isCharged)
        {
            cauldronAudioSource.PlayOneShot(flameSound);
            isLit = true;
            fire.SetActive(true);
        }

    }

    public void TouchEarth()
    {
        if (!hasWood)
        {
            cauldronAudioSource.PlayOneShot(earthSound);
            hasWood = true;
            StartCoroutine(RaiseFireWood(0.5f));
        }
        
    }

    private IEnumerator RaiseFireWood(float riseTime)
    {
        float count = 0;
        while (count < riseTime)
        {
            count += Time.deltaTime;
            wood.transform.position += Vector3.up * 2 * Time.deltaTime / riseTime;
        }
        yield return null;
    }
    public void TouchWind()
    {
        
        if (!hasWind && hasWood && hasWater && isLit)
        {
            cauldronAudioSource.PlayOneShot(windSound);
            hasWind = true;
        }

    }
}
