using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum WindmillDamageState
{
    tangled,
    snapped,
    needsWater,
    canSpin
}
public class Windmill : MonoBehaviour, IElementInteractable
{
    [Header("Windmill")]
    [SerializeField] private float windmillAcceleration = 0f;
    [SerializeField] private float windmillSpeed = 0f;
    [SerializeField] private float maxWindmillSpeed = 100f;
    [SerializeField] private float windmillDrag = 10f;
    [SerializeField] private Transform windmillAxis;
    [SerializeField] private Cloud cloud;
    private string windmillID;
    private float cloudMoveAwayTime = 10;
    public bool isSpinning = false;
    private AudioSource source;

    [Header("State & Interaction")]
    [SerializeField] public WindmillDamageState currentState = WindmillDamageState.canSpin;

    [Header("Specifics")]
    [SerializeField] private GameObject trees;
    [SerializeField] private GameObject flames;
    [SerializeField] private GameObject smoke;


    void Start()
    {
        source = GetComponent<AudioSource>();
        if (trees && flames && smoke)
        {
            trees.SetActive(true);
            flames.SetActive(false);
            smoke.SetActive(false);
        }

        switch (currentState)
        {
            case WindmillDamageState.tangled:
                windmillID = "WindmillTangled";
                break;

            case WindmillDamageState.snapped:
                windmillID = "WindmillBroken";
                break;

            case WindmillDamageState.needsWater:
                windmillID = "WindmillWater";
                break;
        }

    }

    void Update()
    {
        windmillAcceleration = Mathf.Max(0, windmillAcceleration - Time.deltaTime * windmillDrag);
        windmillSpeed = Mathf.Min(windmillSpeed + windmillAcceleration, maxWindmillSpeed);
        windmillSpeed = Mathf.Max(windmillSpeed - Time.deltaTime * windmillDrag * windmillDrag, isSpinning ? maxWindmillSpeed / 2f : 0f);
        
        windmillAxis.Rotate(Vector3.up, windmillSpeed * Time.deltaTime, Space.Self);

        if (currentState == WindmillDamageState.canSpin && windmillSpeed > maxWindmillSpeed / 2 && !isSpinning)
        {
            source.Play();
            isSpinning = true;
            CameraManager.main.ActivateCamera(windmillID);
            StartCoroutine(CameraManager.main.DisableCameraAfterXSeconds(windmillID, cloudMoveAwayTime));
            StartCoroutine(cloud.MoveCloud((cloud.transform.position - transform.position).normalized, cloudMoveAwayTime));
        }
    }

    //Called when the windmill has been repaired
    public void ReadyToSpin()
    {
        currentState = WindmillDamageState.canSpin;
    }
    
    //Toggles the water windmill's ability to spin.
    public void IsReceivingPowerFromWheel(bool isPowered)
    {
        if (isPowered && currentState == WindmillDamageState.needsWater)
        {
            currentState = WindmillDamageState.canSpin;
        }
        else if (!isPowered && currentState == WindmillDamageState.canSpin)
        {
            currentState = WindmillDamageState.needsWater;
        }
    }
    public void TouchEarth()
    {
        
    }

    public void TouchFire(bool isCharged)
    {
        if (currentState == WindmillDamageState.tangled && isCharged) //isCharged prevents contact alone igniting the trees
        {
            StartCoroutine(BurnTrees());
        }
    }
    IEnumerator BurnTrees()
    {
        flames.SetActive(true);
        yield return new WaitForSeconds(2f);
        trees.SetActive(false);
        smoke.SetActive(true);
        currentState = WindmillDamageState.canSpin;
    }

    public void TouchWater()
    {
        
    }

    public void TouchWind()
    {
        if (currentState == WindmillDamageState.canSpin)
            windmillAcceleration += 0.1f;
    }

    
    

    
}
