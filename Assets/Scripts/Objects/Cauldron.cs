using UnityEngine;

public class Cauldron : MonoBehaviour, IElementInteractable
{
    private bool isLit = false, hasWater = false, hasWind = false, hasEarth = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActive())
        {
            // spawn bridge to boss
        }
    }

    public bool IsActive()
    {
        return isLit && hasWater && hasWind && hasEarth;
    }

    public void TouchWater()
    {
        hasWater = true;
    }

    public void TouchFire()
    {
        if (hasEarth)
        {
            isLit = true;
            //add flames to fire
        }

    }

    public void TouchEarth()
    {
        hasEarth = true;
        //add wood to fire
    }

    public void TouchWind()
    {
        if (hasEarth && hasWater && isLit) //wind is the last thing
        {
            hasWind = true;
            //trigger platforms
        }

    }
}
