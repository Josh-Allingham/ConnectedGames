using UnityEngine;

//Derived class from element that records the Chaos Status
public class Chaos : Element
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get the Chaos stats and moves
        getPlayerStats("Chaos");
        getPlayerMoves("Chaos");
    }

    // Update is called once per frame
    void Update()
    {
        //Ensures the sprite is facing the camera
        transform.LookAt(transform.position - (Camera.main.transform.position - transform.position));
    }
}
