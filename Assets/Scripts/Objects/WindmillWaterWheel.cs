using UnityEngine;

public class WindmillWaterWheel : MonoBehaviour, IElementInteractable
{
    [SerializeField] private Windmill windmill;
    [Header("Windmill")]
    [SerializeField] private float wheelAcceleration = 0f;
    [SerializeField] private float wheelSpeed = 0f;
    [SerializeField] private float maxWheelSpeed = 100f;
    [SerializeField] private float wheelDrag = 10f;
    
    void Start()
    {
        
    }

    void Update()
    {
        wheelAcceleration = Mathf.Max(0, wheelAcceleration - Time.deltaTime * wheelDrag);
        wheelSpeed = Mathf.Min(wheelSpeed + wheelAcceleration, maxWheelSpeed);
        wheelSpeed = Mathf.Max(wheelSpeed - Time.deltaTime * wheelDrag * wheelDrag, 0f);

        transform.Rotate(Vector3.right, wheelSpeed * Time.deltaTime, Space.Self);

        
        windmill.IsReceivingPowerFromWheel(wheelSpeed > maxWheelSpeed / 2);
    }
    public void TouchEarth()
    {
        
    }

    public void TouchFire()
    {
        
    }

    public void TouchWater()
    {
        wheelAcceleration += 1f;

    }

    public void TouchWind()
    {
        
    }

   
}
