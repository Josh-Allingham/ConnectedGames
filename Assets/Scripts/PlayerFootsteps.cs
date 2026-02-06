using UnityEngine;
public class PlayerFootsteps : MonoBehaviour
{
    public float footstepRadius = 1f;
    public Player.PlayerType playerType;
    public Color footstepColour;

    #region ColorCodes

    public Color FIRE;
    public Color WATER;
    public Color EARTH;
    public Color MUD;
    public Color DRY;
    public Color MAGMA;
    #endregion

    void Start()
    {
        
        

    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.FloorToInt(Time.time) % 5 == 0)
            CheckTerrain();
    }

    private void CheckTerrain()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, .5f))
        {
            hit.collider.gameObject.TryGetComponent(out GroundTextureGenerator generator);
            if (generator != null)
            {
                //If standing on drawable texture


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

    private Vector2Int GetPositionOnTexture(GroundTextureGenerator generator)
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

    private void ApplyWaterFootsteps(GroundTextureGenerator generator)
    {
        //Our location
        Vector2Int drawPos = GetPositionOnTexture(generator);

        //Current colour of the ground
        Color currentColour = generator.GetColorAt(drawPos.x, drawPos.y);

        //By default set our paint colour to the colour of our type
        Color colour = WATER;
        if (currentColour == FIRE)
        {
            //TODO ReleaseSteam()
            Debug.Log("W/F");
            //dry out spot
            colour = DRY;
        } 
        else if (currentColour == EARTH)
        {
            Debug.Log("W/E");
            colour = MUD;
        }

        //Draw at location
        generator.DrawAt(drawPos.x, drawPos.y, (int)footstepRadius, colour);
    }

    private void ApplyFireFootsteps(GroundTextureGenerator generator)
    {
        //Our location
        Vector2Int drawPos = GetPositionOnTexture(generator);

        //Current colour of the ground
        Color currentColour = generator.GetColorAt(drawPos.x, drawPos.y);

        //By default set our paint colour to the colour of our type
        Color colour = FIRE;

        if (currentColour == WATER)
        {
            //TODO ReleaseSteam()
            Debug.Log("F/W");
            //dry out spot
            colour = DRY;
        }
        else if (currentColour == EARTH)
        {
            Debug.Log("F/E");
            colour = MAGMA;
        }
        
        //Draw at location
        generator.DrawAt(drawPos.x, drawPos.y, (int)footstepRadius, colour);
    }

    private void ApplyEarthFootsteps(GroundTextureGenerator generator)
    {
        //Our location
        Vector2Int drawPos = GetPositionOnTexture(generator);

        //Current colour of the ground
        Color currentColour = generator.GetColorAt(drawPos.x, drawPos.y);

        //By default set our paint colour to the colour of our type
        Color colour = EARTH;

        //TODO Quake()
        if (currentColour == WATER)
        {
            Debug.Log("E/W");
            colour = MUD;
        }
        else if (currentColour == FIRE)
        {
            Debug.Log("E/F");
            colour = MAGMA;
        }

        //Draw at location
        generator.DrawAt(drawPos.x, drawPos.y, (int)footstepRadius, colour);
    }

    private void ApplyWindFootsteps(GroundTextureGenerator generator)
    {

    }
}
