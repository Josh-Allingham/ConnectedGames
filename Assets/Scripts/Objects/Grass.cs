using UnityEngine;

public class Grass : MonoBehaviour, IElementInteractable
{
    [SerializeField]
    private States currentState = States.Base;
    private Color currentColour = Color.black;
    void Start()
    {

    }

    void Update()
    {
        
        updateColour();
        
        
    }
    public void updateColour()
    {
        GetComponent<Renderer>().material.color = currentColour;
        switch (currentState)
        {
            case States.Base:
                currentColour = Color.green;
                return;
            case States.Soaked:
                currentColour = Color.blue;
                return;
            case States.Ablaze:
                currentColour = Color.red;
                return;
            case States.Dead:
                currentColour = Color.gray;
                return;
            case States.Scorched:
                currentColour = new Color(150f / 255f, 75f / 255f, 0);
                return;
            default:
                currentColour = Color.black;
                return;
        }
    }
    public void TouchEarth()
    {
        
        switch (currentState)
        {
            case States.Base:
                currentState = States.Dead;
                return;
            case States.Soaked:
                return;
            case States.Ablaze:
                currentState = States.Scorched;
                return;
            case States.Dead:
                return;
            case States.Scorched:
                return;
        }
    }

    public void TouchFire(bool isCharged)
    {
        switch (currentState)
        {
            case States.Base:
                currentState = States.Ablaze;
                return;
            case States.Soaked:
                //release steam
                currentState = States.Base;
                return;
            case States.Ablaze:
                return;
            case States.Dead:
                currentState = States.Ablaze;
                return;
            case States.Scorched:
                currentState = States.Ablaze;
                return;
        }
    }

    public void TouchWater()
    {
        switch (currentState)
        {
            case States.Base:
                currentState = States.Soaked;
                return;
            case States.Soaked:
                return;
            case States.Ablaze:
                currentState = States.Scorched;
                return;
            case States.Dead:
                currentState = States.Base;
                return;
            case States.Scorched:
                currentState = States.Base;
                return;
        }
    }

    public void TouchWind()
    {
        
    }

    enum States
    {
        Base,
        Soaked,
        Ablaze,
        Scorched,
        Dead
    }
}
