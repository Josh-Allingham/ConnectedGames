using UnityEngine;

//Derived class from element that records the Water Status
public class Water : Element
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get the Water stats and moves
        getPlayerStats("Water");
        getPlayerMoves("Water");
    }

    // Update is called once per frame
    void Update()
    {
        //Ensures the sprite is facing the camera
        transform.LookAt(transform.position - (Camera.main.transform.position - transform.position));
    }
}
