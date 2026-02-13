using UnityEngine;
using System.Collections;
public class WindTunnel : MonoBehaviour
{
    [SerializeField] private float upwardsForce = 1f;
    [SerializeField] private float lifetimeInSeconds = 2f;

    private void Awake()
    {
        StartCoroutine("LifetimeCountdown", lifetimeInSeconds);
    }
    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out Player player);
        if (player != null)
        {
            player.GetComponent<Rigidbody>().AddForce(Vector3.up * upwardsForce, ForceMode.Impulse);
        }
    }

    private IEnumerator LifetimeCountdown(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);

        Destroy(transform.parent.gameObject);
    }
}
