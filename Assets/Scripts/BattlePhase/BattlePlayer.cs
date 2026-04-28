using Photon.Pun;
using UnityEngine;

//The purpose of this class is to record and track all the status of the element the player controls as well as update other players of its current state participating in the battle
public class BattlePlayer : MonoBehaviourPunCallbacks
{
    public string playerName;
    public string playerElement;
    public bool spawned;

    [SerializeField]
    public GameObject water, fire, earth, wind;

    public void Start()
    {
        RPCSetPlayerName(playerName);
        RPCSetPlayerElement(playerElement);
        spawnElemental();
        positionElemental();
    }

    public void Update()
    {
        RPCSetPlayerName(playerName);
        RPCSetPlayerElement(playerElement);
    }

    //Update everyone and itself of the set player name of this Battle Player
    [PunRPC]
    public void RPCSetPlayerName(string name)
    {
        playerName = name;
        if (photonView.IsMine)
        {
            photonView.RPC("RPCSetPlayerName", RpcTarget.OthersBuffered, name);
        }
    }

    //Update everyone and itself of the set player element of this Battle Player
    [PunRPC]
    public void RPCSetPlayerElement(string element)
    {
        playerElement = element;
        if (photonView.IsMine)
        {
            photonView.RPC("RPCSetPlayerElement", RpcTarget.OthersBuffered, element);
        }
    }

    //Spawns the element dependant on what element this Battle Player is controlling
    public virtual void spawnElemental()
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
        }
    }

    //Positions the element dependant on what element this Battle Player is controlling
    public virtual void positionElemental()
    {
        switch (playerElement)
        {
            case "Water":
                this.transform.position = new Vector3 (4.16f, 2.17f, -8.95f);
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
        }

    }
}
