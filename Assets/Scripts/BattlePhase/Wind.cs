using UnityEngine;

public class Wind : Element
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        getPlayerStats("http://localhost/CGDB/WindStats.php");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
