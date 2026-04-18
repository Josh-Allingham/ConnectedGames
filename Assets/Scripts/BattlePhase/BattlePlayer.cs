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
        RPCSpawnElemental();
        RPCPositionElemental();
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


    public void RPCSpawnElemental()
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

    public void RPCPositionElemental()
    {
        switch (playerElement)
        {
            case "Water":
                this.transform.position = new Vector3 (-0.289f, 2.17f, -10.26f);
                break;
            case "Fire":
                this.transform.position = new Vector3(-6.02f, 1.97f, -6.49f);
                break;
            case "Earth":
                this.transform.position = new Vector3(7.045f, 2.57f, -5.074f);
                break;
            case "Wind":
                this.transform.position = new Vector3(11.145f, 2.39f, -8.15f);
                break;
        }

    }
}
