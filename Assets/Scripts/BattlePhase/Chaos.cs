using UnityEngine;

public class Chaos : Element
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        getPlayerStats("Chaos");
        getPlayerMoves("Chaos");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position - (Camera.main.transform.position - transform.position));
    }
}
