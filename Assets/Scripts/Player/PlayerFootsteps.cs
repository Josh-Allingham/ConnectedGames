using UnityEngine;
using Photon.Pun;
public class PlayerFootsteps : MonoBehaviour
{


    [Header("Footsteps")]
    public float footstepRadius = 1f;
    public float footstepDistance;
    public float timeBetweenIdleSpawns = 0.2f;
    public bool isActive = false;

    private Color footstepColour;
    private Vector3 prevPosition;
    public Player.PlayerType playerType { get; private set; }
    private float powerTimer = 0;

    private Player player;

    #region Stats
    [SerializeField] private float WindChargeTimeInSeconds = 4f;
    #endregion
    #region ColorCodes
    [Header("Colours")]
    [SerializeField] private Color WATER;
    [SerializeField] private Color burnBlendColour;
    [SerializeField] private Color EARTH;
    #endregion

    #region Particles
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
        if (!isActive)
        {
            powerTimer = 0;
            return;
        }
        powerTimer += Time.deltaTime;
        if (prevPosition == null || (prevPosition - transform.position).magnitude > footstepDistance)
        {
            prevPosition = transform.position;
            PlaceFootstep();
        }
        else if (GetComponent<Rigidbody>().linearVelocity.magnitude <= .1f && Time.time - Mathf.FloorToInt(Time.time) < timeBetweenIdleSpawns) //If standing still, place periodically
        {
            PlaceFootstep();
        }

    }

    private void PlaceFootstep()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, .5f))
        {
            hit.collider.gameObject.TryGetComponent(out GroundTextureGenerator generator);
            if (generator != null)
            {
                //If standing on drawable texture, run relevent footstep logic

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
    }

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
        
        SpawnParticles(waterParticles, Vector3.one * 0.1f);
        
        generator.DrawAt(drawPos.x, drawPos.y, (int)footstepRadius, colour);
    }
    private void ApplyFireFootsteps(GroundTextureGenerator generator)
    {
        Vector2Int drawPos = GetPlayerPositionOnGrid(generator);

        SpawnParticles(flameParticles, Vector3.one * 0.1f * Mathf.Min(powerTimer, 3));
       
        generator.DrawAt(drawPos.x, drawPos.y, (int)footstepRadius, burnBlendColour, true); //Multiply mode (burn)
    }
    
    private void ApplyEarthFootsteps(GroundTextureGenerator generator)
    {
        //Our location
        Vector2Int drawPos = GetPlayerPositionOnGrid(generator);

        //Current colour of the ground
        Color currentColour = generator.GetColorAt(drawPos.x, drawPos.y);

        //By default set our paint colour to the colour of our type
        Color colour = EARTH;

        //TODO Quake()
        
        SpawnParticles(earthParticles, Vector3.one * 0.1f);
        //Draw at location
        generator.DrawAt(drawPos.x, drawPos.y, (int)footstepRadius, colour);
    }
    private void ApplyWindFootsteps(GroundTextureGenerator generator)
    {
        SpawnParticles(windParticles, Vector3.one * 0.1f * Mathf.Min(powerTimer / 2, 2));
        if (powerTimer >= WindChargeTimeInSeconds)
        {
            player.SpawnWindTunnel(powerTimer);
            powerTimer = 0;
        }
        
        
    }

    
}
