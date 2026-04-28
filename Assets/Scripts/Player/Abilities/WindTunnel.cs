using UnityEngine;
using System.Collections;
public class WindTunnel : MonoBehaviour
{
    [SerializeField] private float upwardsForce = 10f;
    [SerializeField] private float lifetimeInSeconds = 2f;

    private void Awake()
    {
        StartCoroutine("LifetimeCountdown", lifetimeInSeconds);
    }

    private void boostUpwards(Player player)
    {
        player.GetComponent<Rigidbody>().AddForce(Vector3.up * upwardsForce, ForceMode.Impulse);
    }
    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out Player player);
        if (player != null)
        {
            boostUpwards(player);
        }
        other.TryGetComponent(out IElementInteractable obj);
        if (obj != null)
        {
            obj.TouchWind();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        other.TryGetComponent(out IElementInteractable obj);
        if (obj != null)
        {
            obj.TouchWind();
        }
        if (other.TryGetComponent(out Player player))
        {
            boostUpwards(player);
        }
    }
    private IEnumerator LifetimeCountdown(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);

        Destroy(transform.parent.gameObject);
    }
}
