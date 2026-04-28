using UnityEngine;

//Derived class from element that records the Wind Status
public class Wind : Element
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get the Wind stats and moves
        getPlayerStats("Wind");
        getPlayerMoves("Wind");
    }

    // Update is called once per frame
    void Update()
    {
        //Ensures the sprite is facing the camera
        transform.LookAt(transform.position - (Camera.main.transform.position - transform.position));
    }
}
