using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections.Generic;
public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public enum PlayerType
    {
        Water,
        Fire,
        Earth,
        Wind,
        Chaos,
        NULL
    };

    #region spriteIcons;
    [SerializeField] public Sprite waterIcon, fireIcon, earthIcon, windIcon;
    #endregion
    public PlayerType currentType;
    public Color currentColour;
    public string playerName;

    //Wind
    public GameObject windTunnel;
    public GameObject windTunnelInstance;
    public GameObject earthCube;
    public List<GameObject> earthCubeInstances;
    void Start()
    {
        AssignElementColour();
        GetComponent<PlayerFootsteps>().SetType(currentType);
        GetComponentInChildren<SpriteRenderer>().sprite = waterIcon;
        RPCSetPlayerName(playerName);
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ToggleElementSwitch();
                AssignElementColour();
            }
            
            RPCSetElementPowerActive(Input.GetKey(KeyCode.E));
            RPCSetPlayerName(playerName);
        }
        
    }

    [PunRPC]  void RPCSetPlayerName(string _playerName)
    {
        GetComponentInChildren<TMP_Text>().text = _playerName;

        if (photonView.IsMine)
        {
            photonView.RPC("RPCSetPlayerName", RpcTarget.OthersBuffered, _playerName);
        }
    }
    [PunRPC]  void RPCChangeColourTo(Vector3 colour)
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
    [PunRPC] void RPCSetElementPowerActive(bool isActive)
    {
        GetComponent<PlayerFootsteps>().isActive = isActive;

       
        //Tell everyone else what our new colour is
        if (photonView.IsMine)
        {
            photonView.RPC("RPCSetElementPowerActive", RpcTarget.OthersBuffered, isActive);
        }
    }
    [PunRPC] void RPCChangeTypeTo(PlayerType newType)
    {

        currentType = newType;
        GetComponent<PlayerFootsteps>().SetType(currentType);

        Sprite sprite = waterIcon;
        switch (currentType)
        {
            case PlayerType.Water:
                sprite = waterIcon;
                break;
            case PlayerType.Fire:
                sprite = fireIcon;
                break;
            case PlayerType.Earth:
                sprite = earthIcon;
                break;
            case PlayerType.Wind:
                sprite = windIcon;
                break;
        }

        GetComponentInChildren<SpriteRenderer>().sprite = sprite;
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
    public void SpawnWindTunnel()
    {
        windTunnelInstance = Instantiate(windTunnel, transform.position, Quaternion.identity);
        //windTunnelInstance.transform.localScale = Vector3.one * Mathf.Min(powerTimer, 3);
    }

    public void SpawnEarthCube()
    {
        GameObject newCube = Instantiate(earthCube, transform.position + Vector3.right, Quaternion.identity);
        earthCubeInstances.Add(newCube);
    }
    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent<IElementInteractable>(out IElementInteractable enviroObject);
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
