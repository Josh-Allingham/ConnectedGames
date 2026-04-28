using UnityEngine;

//Derived class from element that records the Earth status
public class Earth : Element
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get the Earth stats and moves
        getPlayerStats("Earth");
        getPlayerMoves("Earth");
    }

    // Update is called once per frame
    void Update()
    {
        //Ensures the sprite is facing the camera
        transform.LookAt(transform.position - (Camera.main.transform.position - transform.position));
    }
}
