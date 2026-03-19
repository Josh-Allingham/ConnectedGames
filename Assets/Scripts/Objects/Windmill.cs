using UnityEngine;

public class Windmill : MonoBehaviour, IElementInteractable
{
    [Header("Windmill")]
    [SerializeField] private float windmillAcceleration = 0f;
    [SerializeField] private float windmillSpeed = 0f;
    [SerializeField] private float maxWindmillSpeed = 100f;
    [SerializeField] private float windmillDrag = 10f;
    [SerializeField] private Transform windmillAxis;
    private enum WindmillDamageState
    {
        tangled,
        snapped,
        needsWater,
        NULL
    }

    [Header("State & Interaction")]
    [SerializeField] private WindmillDamageState currentState = WindmillDamageState.NULL;
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

            case WindmillDamageState.NULL:
                r.material.color = Color.white;
                break;
        }
    }

    public void IsReceivingPowerFromWheel(bool isPowered)
    {
        if (isPowered && currentState == WindmillDamageState.needsWater)
        {
            currentState = WindmillDamageState.NULL;
        }else if (!isPowered && currentState == WindmillDamageState.NULL)
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
            currentState = WindmillDamageState.NULL;
            
            for (int i = 0; i < 10; i++)
                Instantiate(flameParticles, windmillAxis.position + Random.insideUnitSphere, Quaternion.identity);
        }
    }

    public void TouchWater()
    {
        
    }

    public void TouchWind()
    {
        if (currentState == WindmillDamageState.NULL)
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
                currentState = WindmillDamageState.NULL;
            }
        }
    }
}
