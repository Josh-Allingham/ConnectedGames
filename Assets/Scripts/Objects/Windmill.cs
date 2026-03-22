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
        windmillSpeed = Mathf.Max(windmillSpeed - Time.deltaTime * windmillDrag * windmillDrag, 0f);

        windmillAxis.Rotate(Vector3.up, windmillSpeed * Time.deltaTime, Space.Self);

        Renderer r = windmillAxis.GetComponent<Renderer>();
        switch (currentState)
        {
            case WindmillDamageState.tangled:
                r.material.color = Color.green;
                break;

            case WindmillDamageState.snapped:
                r.material.color = Color.gray;
                break;

            case WindmillDamageState.needsWater:
                r.material.color = Color.yellow;
                break;

            case WindmillDamageState.canSpin:
                r.material.color = Color.white;
                if (windmillSpeed > maxWindmillSpeed / 2)
                {
                    StartCoroutine(cloud.MoveCloud((cloud.transform.position - transform.position).normalized, 10));
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
