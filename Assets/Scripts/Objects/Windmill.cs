using UnityEngine;

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
    private enum WindmillDamageState
    {
        tangled,
        snapped,
        needsWater,
        canSpin
    }

    [Header("State & Interaction")]
    [SerializeField] private WindmillDamageState currentState = WindmillDamageState.canSpin;
    public ParticleSystem flameParticles;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        windmillAcceleration = Mathf.Max(0, windmillAcceleration - Time.deltaTime * windmillDrag);
        windmillSpeed = Mathf.Min(windmillSpeed + windmillAcceleration, maxWindmillSpeed);
        windmillSpeed = Mathf.Max(windmillSpeed - Time.deltaTime * windmillDrag * windmillDrag, isSpinning ? maxWindmillSpeed / 2f : 0f);
        
        windmillAxis.Rotate(Vector3.up, windmillSpeed * Time.deltaTime, Space.Self);

        
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

            case WindmillDamageState.canSpin:

                if (windmillSpeed > maxWindmillSpeed / 2)
                {
                    isSpinning = true;
                    CameraManager.main.ActivateCamera(windmillID);
                    StartCoroutine(CameraManager.main.DisableCameraAfterXSeconds(windmillID, cloudMoveAwayTime, "Player"));
                    StartCoroutine(cloud.MoveCloud((cloud.transform.position - transform.position).normalized, cloudMoveAwayTime));
                    
                }
                break;
        }
    }

    public void ReadyToSpin()
    {
        currentState = WindmillDamageState.canSpin;
    }
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

    public void TouchFire()
    {
        if (currentState == WindmillDamageState.tangled)
        {
            currentState = WindmillDamageState.canSpin;
            
            for (int i = 0; i < 10; i++)
                Instantiate(flameParticles, windmillAxis.position + Random.insideUnitSphere, Quaternion.identity);
        }
    }

    public void TouchWater()
    {
        
    }

    public void TouchWind()
    {
        if (currentState == WindmillDamageState.canSpin)
            windmillAcceleration += 0.1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentState == WindmillDamageState.snapped && other.TryGetComponent(out Player player))
        {
            Debug.Log(player);
            //UI display press R to repair
            if (Input.GetKey(KeyCode.R) && player.currentType == Player.PlayerType.Earth)
            {
                ReadyToSpin();
                
            }
        }
    }
}
