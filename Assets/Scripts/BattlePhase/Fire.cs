using UnityEngine;

public class Fire : Element
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        getPlayerStats("http://localhost/CGDB/FireStats.php");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
