using UnityEngine;
using System.Collections.Generic;

public class EarthCube : MonoBehaviour, IElementInteractable
{

    private float riseTime = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (riseTime > 0)
        {
            Rise();
            riseTime--;
        }
        else
        {

        }
            
    }

    void Rise()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime, transform.position.z);
    }

    public void TouchWater()
    {
        
    }

    public void TouchFire()
    {
        
    }

    public void TouchEarth()
    {
        
    }

    public void TouchWind()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Impulse);
    }
}
