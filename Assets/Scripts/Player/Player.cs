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
    [SerializeField] public Sprite waterSprite, fireSprite, earthSprite, windSprite;
    #endregion
    public PlayerType currentType;
    public Color currentColour;
    public string playerName;
    public SpriteRenderer iconSprite;
    public SpriteRenderer bodySprite;

    //Wind
    public GameObject windTunnel;
    public GameObject windTunnelInstance;
    public GameObject earthCube;
    public List<GameObject> earthCubeInstances;

    private Animator anim;
    private Rigidbody rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
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
            }
            
            RPCSetElementPowerActive(Input.GetKey(KeyCode.E));
            RPCSetPlayerName(playerName);

            anim.SetBool("IsWalking", rb.linearVelocity.magnitude > .1f);
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

        GetComponentsInChildren<SpriteRenderer>()[1].color = currentColour;

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

        Sprite iconSprite = waterIcon;
        switch (currentType)
        {
            case PlayerType.Water:
                iconSprite = waterIcon;
                bodySprite.sprite = waterSprite;
                anim.SetInteger("ElementID", 0);
                break;
            case PlayerType.Fire:
                iconSprite = fireIcon;
                bodySprite.sprite = fireSprite;
                anim.SetInteger("ElementID", 1);
                break;
            case PlayerType.Earth:
                iconSprite = earthIcon;
                bodySprite.sprite = earthSprite;
                anim.SetInteger("ElementID", 2);
                break;
            case PlayerType.Wind:
                iconSprite = windIcon;
                bodySprite.sprite = windSprite;
                anim.SetInteger("ElementID", 3);
                break;
        }

        GetComponentInChildren<SpriteRenderer>().sprite = iconSprite;
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
                RPCChangeColourTo(new Vector3(142f / 255f, 147f / 255f, 1));
                return;
            case PlayerType.Fire:
                RPCChangeColourTo(new Vector3(1, 185f/255f, 117f/255f));
                return;
            case PlayerType.Earth:
                RPCChangeColourTo(new Vector3(185f / 255f, 1, 117f / 255f));
                return;
            case PlayerType.Wind:
                RPCChangeColourTo(new Vector3(1, 1, 1));
                return;
        }
    }
    void ToggleElementSwitch()
    {
        anim.SetInteger("ElementID", -1);
        anim.SetTrigger("Switch");

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
        GameObject newCube = PhotonNetwork.Instantiate(earthCube.name, transform.position + Vector3.down, Quaternion.identity);
        earthCubeInstances.Add(newCube);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IElementInteractable enviroObject))
        {
            if (currentType == PlayerType.Fire)
            {
                enviroObject.TouchFire();
            }
        }
        /*if (enviroObject != null)
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
        }*/
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
