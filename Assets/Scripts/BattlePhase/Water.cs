using UnityEngine;

public class Water : Element
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.ElementType = "Water";
        this.CurrentHealth = 50;
        this.MaxHealth = 50;
        this.Defence = 25;
        this.Attack = 25;
        this.Speed = 5;
        this.ElementStatera = 50;
        this.IsAlive = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
