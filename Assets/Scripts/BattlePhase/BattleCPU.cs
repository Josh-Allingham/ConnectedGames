using Photon.Pun;
using UnityEngine;

public class BattleCPU : BattlePlayer 
{
    public GameObject chaos;

    public override void spawnElemental()
    {
        switch (playerElement)
        {
            case "Water":
                Instantiate(water, transform.position, Quaternion.identity, this.transform);
                break;
            case "Fire":
                Instantiate(fire, transform.position, Quaternion.identity, this.transform);
                break;
            case "Earth":
                Instantiate(earth, transform.position, Quaternion.identity, this.transform);
                break;
            case "Wind":
                Instantiate(wind, transform.position, Quaternion.identity, this.transform);
                break;
            case "Chaos":
                Instantiate(chaos, transform.position, Quaternion.identity, this.transform);
                break;
        }
    }

    public override void positionElemental()
    {
        switch (playerElement)
        {
            case "Water":
                this.transform.position = new Vector3(4.16f, 2.17f, -8.95f);
                break;
            case "Fire":
                this.transform.position = new Vector3(1.24f, 1.97f, -4.13f);
                break;
            case "Earth":
                this.transform.position = new Vector3(7.045f, 2.57f, -5.074f);
                break;
            case "Wind":
                this.transform.position = new Vector3(10.16f, 2.39f, -8.15f);
                break;
            case "Chaos":
                this.transform.position = new Vector3(5.5f, 4.3f, 7f);
                break;
        }

    }

}

