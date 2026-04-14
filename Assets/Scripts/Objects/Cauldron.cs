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
        isLit = true;
    }

    public void TouchEarth()
    {
        hasEarth = true;
    }

    public void TouchWind()
    {
        hasWind = true;
    }
}
