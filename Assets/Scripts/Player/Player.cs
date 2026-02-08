using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public enum PlayerType
    {
        Water,
        Fire,
        Earth,
        Wind,
        NULL
    };

    public PlayerType currentType;
    public Color currentColour;
    void Start()
    {
        AssignElementColour();
        GetComponent<PlayerFootsteps>().SetType(currentType);
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleElementSwitch();
                AssignElementColour();
            }
        }
        
    }

    [PunRPC]
    void RPCChangeColourTo(Vector3 colour)
    {
        //Change our colour
        currentColour = new Color(colour.x, colour.y, colour.z, 1f);

        GetComponent<PlayerFootsteps>().SetColour(currentColour);

        GetComponent<Renderer>().material.color = currentColour;

        //Tell everyone else what our new colour is
        if (photonView.IsMine)
        {
            photonView.RPC("RPCChangeColourTo", RpcTarget.OthersBuffered, colour);
        }
    }

    [PunRPC]
    void RPCChangeTypeTo(PlayerType newType)
    {
        currentType = newType;
        GetComponent<PlayerFootsteps>().SetType(currentType);
        //Tell everyone else what our new type is
        if (photonView.IsMine)
        {
            photonView.RPC("RPCChangeTypeTo", RpcTarget.OthersBuffered, newType);
        }
    }
    void AssignElementColour()
    {
        switch (currentType)
        {
            case PlayerType.Water:
                RPCChangeColourTo(new Vector3(0,0,1));
                return;
            case PlayerType.Fire:
                RPCChangeColourTo(new Vector3(1, 0, 0));
                return;
            case PlayerType.Earth:
                RPCChangeColourTo(new Vector3(0, 1, 0));
                return;
            case PlayerType.Wind:
                RPCChangeColourTo(new Vector3(1, 1, 1));
                return;
        }
    }
    void ToggleElementSwitch()
    {
        switch (currentType)
        {
            case PlayerType.Water:
                RPCChangeTypeTo(PlayerType.Fire);
                break;
            case PlayerType.Fire:
                RPCChangeTypeTo(PlayerType.Earth);
                break;
            case PlayerType.Earth:
                RPCChangeTypeTo(PlayerType.Wind);
                break;
            case PlayerType.Wind:
                RPCChangeTypeTo(PlayerType.Water);
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent<IEnvironmentDynamic>(out IEnvironmentDynamic enviroObject);
        if (enviroObject != null)
        {
            switch (currentType)
            {
                case PlayerType.Water:
                    enviroObject.TouchWater();
                    return;
                case PlayerType.Fire:
                    enviroObject.TouchFire();
                    return;
                case PlayerType.Earth:
                    enviroObject.TouchEarth();
                    return;
                case PlayerType.Wind:
                    enviroObject.TouchWind();
                    return;
            }

        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send data (local player)
            stream.SendNext(currentType);
        }
        else
        {
            // Receive data (remote players)
            currentType = (PlayerType)stream.ReceiveNext();
            
        }
    }
}
