using UnityEngine;
using Photon.Pun;
public class PlayerPowers : MonoBehaviour
{
    [Header("Footsteps")]
    public float footstepRadius = 1f;
    public float footstepDistance;
    public float timeBetweenIdleSpawns = 0.2f;
    private Color footstepColour;
    private Vector3 prevPosition;

    [Header("Powers")]
    public bool isActive = false;
    public bool isCharged;
    public Player.PlayerType playerType { get; private set; }
    [SerializeField] private float PowerChargeTimeInSeconds = 4f;
    private float powerTimer = 0;

    private Player player;

    [Header("Audio")]
    public AudioSource playerSource;
    public AudioClip walkGrass;
    public AudioClip walkStone;

    [Header("Particles")]
    public ParticleSystem waterParticles;
    public ParticleSystem flameParticles;
    public ParticleSystem earthParticles;
    public ParticleSystem windParticles;
    public ParticleSystem pSTEAM;
    public ParticleSystem pMUD;
    public ParticleSystem pCYCLONE;
    public ParticleSystem pMAGMA;
    public ParticleSystem pFIRESTORM;
    public ParticleSystem pEARTHQUAKE;
    private ParticleSystem[,] particleInteractionLookup;
    #region ColorCodes
    [Header("Colours")]
    [SerializeField] private Color WATER;
    [SerializeField] private Color burnBlendColour;
    [SerializeField] private Color EARTH;
    #endregion


    void Start()
    {
        player = GetComponent<Player>();
        SetType(player.currentType);
        ConstructLookupTable();
    }

    // Update is called once per frame
    void Update()
    {
        player.SetPowerChargeBar(powerTimer / PowerChargeTimeInSeconds);
        isCharged = powerTimer / PowerChargeTimeInSeconds >= 1;

        if (!isActive)
        {
            powerTimer = 0;
            return;
        }

        
        powerTimer += Time.deltaTime;
        
        if (prevPosition == null || (prevPosition - transform.position).magnitude > footstepDistance)
        {
            prevPosition = transform.position;
            ApplyPowers();
        }
        else if (GetComponent<Rigidbody>().linearVelocity.magnitude <= .1f && Time.time - Mathf.FloorToInt(Time.time) < timeBetweenIdleSpawns) //If standing still, place periodically
        {
            ApplyPowers();
        }

    }

    private void ApplyPowers()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, .5f)) //if stood on ground
        {
            //Footstep Logic (drawing on floor)
            hit.collider.gameObject.TryGetComponent(out GroundTextureGenerator generator);
            if (generator != null)//If standing on drawable texture, run relevent footstep logic
            {
                switch (playerType)
                {
                    case Player.PlayerType.Water:
                        ApplyWaterFootsteps(generator);
                        break;
                    case Player.PlayerType.Fire:
                        ApplyFireFootsteps(generator);
                        break;
                    case Player.PlayerType.Earth:
                        ApplyEarthFootsteps(generator);
                        break;
                    case Player.PlayerType.Wind:
                        ApplyWindFootsteps(generator);
                        break;
                }
                
            }
            
        }
        //Spawn power particles
        switch (playerType)
        {
            case Player.PlayerType.Water:
                SpawnParticles(waterParticles, Vector3.one * .5f);
                break;
            case Player.PlayerType.Fire:
                SpawnParticles(flameParticles, Vector3.one * 1f * Mathf.Min(powerTimer, 3));
                break;
            case Player.PlayerType.Earth:
                SpawnParticles(earthParticles, Vector3.one * 1f);

                //Once charged, spawn platform
                if (isCharged)
                {
                    player.RPCSpawnEarthCube();
                    powerTimer = 0;
                }
                break;
            case Player.PlayerType.Wind:
                SpawnParticles(windParticles, Vector3.one * 1f * Mathf.Min(powerTimer / 2, 2));
                //Once charged, spawn wind tunnel
                if (isCharged)
                {
                    player.SpawnWindTunnel();
                    powerTimer = 0;
                }
                break;
        }
    }

    //If the player is stood on a drawable texture, translates player position to grid coordinates on the texture.
    private Vector2Int GetPlayerPositionOnGrid(GroundTextureGenerator generator)
    {
        Vector3 scale = generator.transform.localScale;
        Vector3 playerLoc = transform.position - (generator.transform.position - scale / 2);
        Vector3 mappedLoc = new Vector3(-playerLoc.x / scale.x * generator.width,
                                        0,
                                        -playerLoc.z / scale.z * generator.height);

        return new Vector2Int(Mathf.FloorToInt(mappedLoc.x), Mathf.FloorToInt(mappedLoc.z));
    }
    public void SetColour(Color colour)
    {
        footstepColour = colour;
    }
    public void SetType(Player.PlayerType _playerType)
    {
        playerType = _playerType;
    }
    void SpawnParticles(ParticleSystem particles, Vector3 scale)
    {
        ParticleSystem newParticle = Instantiate(particles, transform.position + Vector3.back * 0.1f, Quaternion.identity);
        newParticle.transform.localScale = scale;
        newParticle.GetComponent<FootstepParticle>().player = this;

    }
    //Create combination table for particle interactions
    void ConstructLookupTable()
    {
        particleInteractionLookup = new ParticleSystem[4, 4];

        int iWATER = TypeToInt(Player.PlayerType.Water);
        int iFIRE = TypeToInt(Player.PlayerType.Fire);
        int iEARTH = TypeToInt(Player.PlayerType.Earth);
        int iWIND = TypeToInt(Player.PlayerType.Wind);

        particleInteractionLookup[iWATER, iWATER] = null;
        particleInteractionLookup[iFIRE, iFIRE] = null;
        particleInteractionLookup[iEARTH, iEARTH] = null;
        particleInteractionLookup[iWIND, iWIND] = null;
        particleInteractionLookup[iWATER, iFIRE] = pSTEAM;
        particleInteractionLookup[iFIRE, iWATER] = pSTEAM;
        particleInteractionLookup[iWATER, iEARTH] = pMUD;
        particleInteractionLookup[iEARTH, iWATER] = pMUD;
        particleInteractionLookup[iWATER, iWIND] = pCYCLONE;
        particleInteractionLookup[iWIND, iWATER] = pCYCLONE;
        particleInteractionLookup[iFIRE, iEARTH] = pMAGMA;
        particleInteractionLookup[iEARTH, iFIRE] = pMAGMA;
        particleInteractionLookup[iWIND, iFIRE] = pFIRESTORM;
        particleInteractionLookup[iFIRE, iWIND] = pFIRESTORM;
        particleInteractionLookup[iWIND, iEARTH] = pEARTHQUAKE;
        particleInteractionLookup[iEARTH, iWIND] = pEARTHQUAKE;
    }
    
    //converts types to integers in defined mapping
    public int TypeToInt(Player.PlayerType type)
    {
        switch (type)
        {
            case Player.PlayerType.Water:
                return 0;
            case Player.PlayerType.Fire:
                return 1;
            case Player.PlayerType.Earth:
                return 2;
            case Player.PlayerType.Wind:
                return 3;
        }
        //if null
        return -1;
    }
    public ParticleSystem ParticleCrossover(Player.PlayerType TypeA, Player.PlayerType TypeB)
    {
        int indexA = TypeToInt(TypeA);
        int indexB = TypeToInt(TypeB);

        if (particleInteractionLookup == null)
            ConstructLookupTable();

        ParticleSystem particles = particleInteractionLookup[indexA, indexB];

        return particles;
    }
    private void ApplyWaterFootsteps(GroundTextureGenerator generator)
    {
        //Our location
        Vector2Int drawPos = GetPlayerPositionOnGrid(generator);

        Color colour = WATER;
        
        
        
        generator.DrawAt(drawPos.x, drawPos.y, (int)footstepRadius, colour);
    }
    private void ApplyFireFootsteps(GroundTextureGenerator generator)
    {
        Vector2Int drawPos = GetPlayerPositionOnGrid(generator);

        
       
        generator.DrawAt(drawPos.x, drawPos.y, (int)footstepRadius, burnBlendColour, true); //Multiply mode (burn)
    }
    
    private void ApplyEarthFootsteps(GroundTextureGenerator generator)
    {
        //Our location
        Vector2Int drawPos = GetPlayerPositionOnGrid(generator);

        Color colour = EARTH;
        generator.DrawAt(drawPos.x, drawPos.y, (int)footstepRadius, colour);

      
        
    }
    private void ApplyWindFootsteps(GroundTextureGenerator generator)
    {
        
        
        
    }

    
}
