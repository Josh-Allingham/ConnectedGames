using UnityEngine;

//Derived class from element that records the Fire Status
public class Fire : Element
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get the Fire stats and moves
        getPlayerStats("Fire");
        getPlayerMoves("Fire");
    }

    // Update is called once per frame
    void Update()
    {
        //Ensures the sprite is facing the camera
        transform.LookAt(transform.position - (Camera.main.transform.position - transform.position));
    }
}
