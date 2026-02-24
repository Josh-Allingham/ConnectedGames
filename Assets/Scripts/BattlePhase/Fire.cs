using UnityEngine;

public class Fire : Element
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.ElementType = "Fire";
        this.CurrentHealth = 50;
        this.MaxHealth = 50;
        this.Defence = 12;
        this.Attack = 50;
        this.Speed = 10;
        this.ElementStatera = 12;
        this.IsAlive = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
