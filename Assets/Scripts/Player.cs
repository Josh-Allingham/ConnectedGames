using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public PlayerType currentType;
    public Color currentColour;
    void Start()
    {
        TempUpdateColour();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ChangeTypeToNext();
                TempUpdateColour();
            }
        }
        
    }

    [PunRPC]
    void RPCChangeColourTo(Vector3 colour)
    {
        GetComponent<Renderer>().material.color = new Color(colour.x, colour.y, colour.z, 1f);
        if (photonView.IsMine)
        {
            photonView.RPC("RPCChangeColourTo", RpcTarget.OthersBuffered, colour);
        }
    }
    void TempUpdateColour()
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
    void ChangeTypeToNext()
    {
        switch (currentType)
        {
            case PlayerType.Water:
                currentType = PlayerType.Fire;
                return;
            case PlayerType.Fire:
                currentType = PlayerType.Earth;
                return;
            case PlayerType.Earth:
                currentType = PlayerType.Wind;
                return;
            case PlayerType.Wind:
                currentType = PlayerType.Water;
                return;
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

    public enum PlayerType
    {
        Water,
        Fire,
        Earth,
        Wind
    };
}
