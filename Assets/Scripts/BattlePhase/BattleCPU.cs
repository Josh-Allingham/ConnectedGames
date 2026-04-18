using Photon.Pun;
using UnityEngine;

public class BattleCPU : BattlePlayer 
{

    void elementalAction()
    {
        switch(playerElement)
        {
            case "Water":
                waterCPUAction();
                break;
            case "Fire":
                fireCPUAction();
                break;
            case "Earth":
                earthCPUAction();
                break;
            case "Wind":
                windCPUAction();
                break;
        }
    }

    public void waterCPUAction()
    {
        Debug.Log("Heal ally with no current health buff, if they are below 30%");
        Debug.Log("Attack enemy if effective");
        Debug.Log("Heal ally with no current health buff");
    }

    public void fireCPUAction()
    {
        Debug.Log("Attack enemy if effective");
        Debug.Log("Buff ally with no current speed buff");
    }

    public void earthCPUAction()
    {
        Debug.Log("Shield ally with no current shield buff, if they are below 30%");
        Debug.Log("Attack enemy if effective");
        Debug.Log("Shield ally with no current shield buff");
    }

    public void windCPUAction()
    {
        Debug.Log("Attack enemy if effective");
        Debug.Log("Wind cast");
    }
}

