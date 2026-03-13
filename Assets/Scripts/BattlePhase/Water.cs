using UnityEngine;

public class Water : Element
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        getPlayerStats("Water");
        getPlayerMoves("Water");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position - (Camera.main.transform.position - transform.position));
    }
}
