using Photon.Pun;
using UnityEngine;

public class BattlePlayer : MonoBehaviourPunCallbacks
{
    public string playerName;
    public string playerElement;

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

    [PunRPC]
    public void RPCSetPlayerName(string name)
    {
        playerName = name;
        if (photonView.IsMine)
        {
            photonView.RPC("RPCSetPlayerName", RpcTarget.OthersBuffered, name);
        }
    }

    [PunRPC]
    public void RPCSetPlayerElement(string element)
    {
        playerElement = element;
        if (photonView.IsMine)
        {
            photonView.RPC("RPCSetPlayerElement", RpcTarget.OthersBuffered, element);
        }
    }


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
