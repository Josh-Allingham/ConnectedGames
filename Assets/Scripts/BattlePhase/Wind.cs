using UnityEngine;

public class Wind : Element
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.ElementType = "Wind";
        this.CurrentHealth = 50;
        this.MaxHealth = 50;
        this.Defence = 25;
        this.Attack = 12;
        this.Speed = 10;
        this.ElementStatera = 50;
        this.IsAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
