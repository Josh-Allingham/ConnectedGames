using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class BattlePlayer : MonoBehaviourPunCallbacks
{
    public string playerName;
    public string playerElement;

    [SerializeField]
    public GameObject water, fire, earth, wind;

    private void Start()
    {
        RPCSetPlayerName();
        RPCSetPlayerElement();
        RPCSpawnElemental();
        RPCPositionElemental();
    }

    void Update()
    {

    }

    [PunRPC]
    void RPCSetPlayerName()
    {
        playerName = "P1";

        if (photonView.IsMine)
        {
            photonView.RPC("RPCSetPlayerName", RpcTarget.OthersBuffered, playerName);
        }
    }

    [PunRPC]
    void RPCSetPlayerElement()
    {
        playerElement = "Fire";

        if (photonView.IsMine)
        {
            photonView.RPC("RPCSetPlayerElement", RpcTarget.OthersBuffered, playerElement);
        }
    }

    [PunRPC]
    void RPCSpawnElemental()
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

        if(photonView.IsMine)
        {
            photonView.RPC("RPCSpawnElemental", RpcTarget.OthersBuffered);
        }
    }

    [PunRPC]
    void RPCPositionElemental()
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
