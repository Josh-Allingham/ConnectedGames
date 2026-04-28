using UnityEngine;
using UnityEngine.UI;
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

    [Header("Sprites")]
    public Sprite waterIcon, fireIcon, earthIcon, windIcon;
    public Sprite waterSprite, fireSprite, earthSprite, windSprite;
    
    [Header("Canvas")]
    public string playerName;
    public Image iconBGSprite;
    public Image iconFGSprite;

    
    public PlayerType currentType;
    public Color currentColour;
    public SpriteRenderer bodySprite;

    public GameObject windTunnel;
    private GameObject windTunnelInstance;
    public GameObject earthCube;
    private List<GameObject> earthCubeInstances;
    public float earthCubeRiseTimeInSeconds = 2f;

    private Animator anim;
    private Rigidbody rb;
    public PlayerPowers powers;
    public PlayerMovement movement;

    public NPC currentInteractee;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        powers = GetComponent<PlayerPowers>();
        movement = GetComponent<PlayerMovement>();
        
        GetComponent<PlayerPowers>().SetType(currentType);
        GetComponentInChildren<SpriteRenderer>().sprite = waterIcon;
        RPCSetPlayerName(playerName);

        if (photonView.IsMine)
        {
            PlayerUI.main.ActivateHintButton(PlayerUI.main.controlsTutorial);
            PlayerUI.main.ShowTutorialOverlay();
        }
        
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
            HandleNPCInteractions();

            WorldToBattleTransfer.playerName = playerName;
            WorldToBattleTransfer.element = powers.TypeToInt(currentType);
        }
        
    }

    void HandleNPCInteractions()
    {
        if (currentInteractee != null)
        {
            PlayerUI.main.UpdateHighlightText(currentInteractee.highlightText, currentInteractee.transform.position, 1f);
            

            if (Input.GetKeyDown(KeyCode.F) && !currentInteractee.HasFinishedDialogue()) //check the player has F'd and there is valid dialogue stored
            {
                photonView.RPC("RPCShowDialogue", RpcTarget.All, currentInteractee.GetDialogue());
            }
            else if (currentInteractee.HasFinishedDialogue())
            {
                switch (currentInteractee.isEvil)
                {
                    case true: //Bridge Conversation Ended
                        RPCEndDialogue();
                        //START BATTLE SCENE
                        ProgressionManager.main.StartBattleScene();
                        
                        break;

                    case false: //Intro Conversation Ended
                        ProgressionManager.main.ChangeState(ProgressionManager.ProgressionState.PostOldMan);
                        photonView.RPC("RPCEndDialogue", RpcTarget.All);
                        
                        break;
                }
                
            }
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

        GetComponent<PlayerPowers>().SetColour(currentColour);

        GetComponentsInChildren<SpriteRenderer>()[1].color = currentColour;

        //Tell everyone else what our new colour is
        if (photonView.IsMine)
        {
            photonView.RPC("RPCChangeColourTo", RpcTarget.OthersBuffered, colour);
        }
    }
    [PunRPC] void RPCSetElementPowerActive(bool isActive)
    {
        powers.isActive = isActive;

        if (photonView.IsMine)
        {
            photonView.RPC("RPCSetElementPowerActive", RpcTarget.OthersBuffered, isActive);
        }
    }
    [PunRPC] void RPCChangeTypeTo(PlayerType newType)
    {
        //Reset Animations
        anim.SetInteger("ElementID", -1);
        anim.SetTrigger("Switch");

        currentType = newType;
        GetComponent<PlayerPowers>().SetType(currentType);

        switch (currentType)
        {
            case PlayerType.Water:
                SetIconSprite(waterIcon);
                bodySprite.sprite = waterSprite;
                anim.SetInteger("ElementID", 0);
                break;
            case PlayerType.Fire:
                SetIconSprite(fireIcon);
                bodySprite.sprite = fireSprite;
                anim.SetInteger("ElementID", 1);
                break;
            case PlayerType.Earth:
                SetIconSprite(earthIcon);
                bodySprite.sprite = earthSprite;
                anim.SetInteger("ElementID", 2);
                break;
            case PlayerType.Wind:
                SetIconSprite(windIcon);
                bodySprite.sprite = windSprite;
                anim.SetInteger("ElementID", 3);
                break;
        }

        if (photonView.IsMine)
        {
            photonView.RPC("RPCChangeTypeTo", RpcTarget.OthersBuffered, newType);
        }
    }

    public void SetPowerChargeBar(float percentageFilled)
    {
        iconFGSprite.fillAmount = percentageFilled;
    }
    private void SetIconSprite(Sprite sprite)
    {
        iconBGSprite.sprite = sprite;
        iconFGSprite.sprite = sprite;
    }
    public void SetInteractee(NPC interactee)
    {
        this.currentInteractee = interactee;
        if (interactee == null)
        {
            PlayerUI.main.UpdateHighlightText("", Vector3.zero, 0);
        }
        else
        {
            PlayerUI.main.UpdateHighlightText(interactee.highlightText, interactee.transform.position, 1);
        }
    }
    [PunRPC] public void RPCShowDialogue(string dialogue)
    {
        PlayerUI.main.ShowDialogue(dialogue);
        movement.canMove = false;
        
    }
    [PunRPC] public void RPCEndDialogue()
    {
        
        PlayerUI.main.EndDialogue();
        CameraManager.main.SetPlayerCam(true);
        currentInteractee = null;
        movement.canMove = true;
        PlayerUI.main.ActivateHintButton(PlayerUI.main.windmillTutorial);
        PlayerUI.main.ShowTutorialOverlay();
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
        PlayerType[] types = new PlayerType[4];
        List<PlayerType> currentTypesInGame = new List<PlayerType>();
        
        types[0] = PlayerType.Water;
        types[1] = PlayerType.Fire;
        types[2] = PlayerType.Earth;
        types[3] = PlayerType.Wind;
        int typeIndex = -1;

        foreach (GameObject player in NetManager.main.players)
        {
            Debug.Log(player.GetComponent<Player>().currentType);
            currentTypesInGame.Add(player.GetComponent<Player>().currentType);
        }
        Debug.Log(currentTypesInGame);
        for (int i = 0; i < 4; i++)
        {
            if (types[i] == currentType)
                typeIndex = i;
        }
        Debug.Log($"Type index: {typeIndex}");
        for (int i = 1; i < 4; i++)
        {
            int desiredIndex = (typeIndex + i) % 4;
            PlayerType desiredType = types[desiredIndex];
            if (!currentTypesInGame.Contains(desiredType))
            {
                RPCChangeTypeTo(desiredType);
                return;
            }
        }
        


        /*switch (currentType)
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
        }*/
    }
    public void SpawnWindTunnel()
    {
        windTunnelInstance = Instantiate(windTunnel, transform.position, Quaternion.identity);
        //windTunnelInstance.transform.localScale = Vector3.one * Mathf.Min(powerTimer, 3);
    }
    [PunRPC] public void RPCSpawnEarthCube()
    {
        if (photonView.IsMine)
        {
            GameObject newCube = PhotonNetwork.Instantiate(earthCube.name, transform.position + Vector3.down, Quaternion.identity);
            newCube.GetComponent<EarthCube>().riseTime = earthCubeRiseTimeInSeconds;
            earthCubeInstances.Add(newCube);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IElementInteractable enviroObject)) //interactable object
        {
            Debug.Log("Player hit");
            if (currentType == PlayerType.Fire) //fire affects the environment passively
            {
                enviroObject.TouchFire(powers.isCharged);
            }
        }
        if (other.tag == "TutorialJump") //Show jumping tutorial UI
        {
            if (photonView.IsMine)
            {
                PlayerUI.main.ActivateHintButton(PlayerUI.main.jumpTutorial);
                PlayerUI.main.ShowTutorialOverlay();
                
            }
            
        }
        if (other.tag == "TutorialCauldron") //Show Cauldron tutorial UI
        {
            if (photonView.IsMine)
            {
                PlayerUI.main.ActivateHintButton(PlayerUI.main.cauldronTutorial);
                PlayerUI.main.ShowTutorialOverlay();
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



