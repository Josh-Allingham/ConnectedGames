using UnityEngine;

public class Water : Element
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        getPlayerStats("http://localhost/CGDB/WaterStats.php");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
