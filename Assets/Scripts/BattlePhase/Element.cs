using UnityEngine;

public class Element : MonoBehaviour
{
    public string elementType;
    public int currHealth;
    public int maxHealth;
    public int defence;
    public int attack;
    public int speed;
    public int elementStatera;
    public bool alive;


    public string ElementType
    {
        get { return elementType; }
        set { elementType = value; }
    }

    public int CurrentHealth
    {
        get { return currHealth; }
        set { currHealth = value; }
    }

    public int MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }
    public int Defence
    {
        get { return defence; }
        set { defence = value; }
    }
    public int Attack
    {
        get { return attack; }
        set { attack = value; }
    }

    public int Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public int ElementStatera
    {
        get { return elementStatera; }
        set { elementStatera = value; }
    }

    public bool IsAlive
    {
        get { return alive; }
        set { alive = value; }
    }

    public void heal(int aid)
    {   if (alive)
        {
            if ((currHealth + aid) > maxHealth)
            {
                currHealth = maxHealth;
            }
            else
            {
                currHealth += aid;
            }
        }
    }

    public void fullHeal()
    {
        if(alive)
        {
            currHealth = maxHealth;
        }
    }

    public void damage(int dmg)
    {
        if (alive)
        {
            if (currHealth - dmg <= 0)
            {
                currHealth = 0;
                death();
            }
            else
            {
                currHealth -= dmg;
            }
        }
    }

    public void death()
    {
        alive = false;
    }

    public void revive()
    {
        if (!alive)
        {
            alive = true;
            heal(maxHealth / 3);
        }
    }
}