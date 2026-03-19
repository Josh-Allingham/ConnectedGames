using UnityEngine;
using System.Collections;
public class GrassTuft : MonoBehaviour, IElementInteractable
{
    [SerializeField] private Transform grassAnchor;
    [SerializeField] private float maxAngle;
    [SerializeField] private float desiredAngle = 0f;
    [SerializeField] private float springBackMultiplier = 2f;
    private bool isAblaze = false;
    private bool isDead = false;
    [SerializeField] private float burnTimeInSeconds = 3f;
    [SerializeField] private ParticleSystem burnParticle;
    
    void Start()
    {
        burnParticle.GetComponent<FootstepParticle>().InteractionType = Player.PlayerType.NULL;
    }

    void Update()
    {
        Vector3 currentRot = grassAnchor.localEulerAngles;
        float restForce = (desiredAngle - currentRot.z) * Time.deltaTime * springBackMultiplier;
        grassAnchor.localEulerAngles = new Vector3(0,0, currentRot.z + restForce);

        if (isAblaze)
            Instantiate(burnParticle, transform.position, Quaternion.identity);
        if (isDead)
            Destroy(this.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        other.TryGetComponent(out Player player);
        if (player != null)
        {
            Vector3 dirVector = transform.position - player.transform.position;
            float angle = (dirVector.magnitude - 0.35f) / 0.07f;
            angle = Mathf.Lerp(0, maxAngle, angle);
            angle *= dirVector.x >= 0 ? -1 : 1;
            grassAnchor.localEulerAngles = new Vector3(0,0, angle);
        }
    }

    public void TouchWater()
    {
        isAblaze = false;   
    }

    public void TouchFire()
    {
        //burn
        StartCoroutine(Burn());
        
    }

    public void TouchEarth()
    {
        //stick to rock?
    }

    public void TouchWind()
    {
        Debug.Log("H");
        Vector3 currentRot = grassAnchor.localEulerAngles;
        grassAnchor.localEulerAngles = new Vector3(0, 0, currentRot.z + 10 * Mathf.PerlinNoise1D(currentRot.z));
    }

    private IEnumerator Burn()
    {
        isAblaze = true;
        yield return new WaitForSeconds(burnTimeInSeconds);
        if (isAblaze)
        {
            isDead = true;
        }
    }
}
