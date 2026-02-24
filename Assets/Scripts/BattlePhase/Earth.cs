using UnityEngine;

public class Earth : Element
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.ElementType = "Earth";
        this.CurrentHealth = 50;
        this.MaxHealth = 50;
        this.Defence = 50;
        this.Attack = 25;
        this.Speed = 2;
        this.ElementStatera = 25;
        this.IsAlive = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

