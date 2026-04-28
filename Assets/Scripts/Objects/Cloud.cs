using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Cloud : MonoBehaviour
{
    [SerializeField] private GameObject spherePrefab;
    private List<CloudBall> spheres;
    [SerializeField] private Vector2 maxSphereRadiusRange = new Vector2(1f,2f);
    [SerializeField] private int numBalls = 5;
    [SerializeField] private int maxBallRange = 3;
    [SerializeField] private float speedMultiplier = 0.5f;
    public bool isAlive = true;
    private float timer = 0;

    public AudioClip cloudMove;
    void Start()
    {
        /*spheres = new List<CloudBall>();

        for (int i = 0; i < numBalls; i++)
        {
            AddBall();
        }*/
    }

    void Update()
    {
        /*UpdateBalls();
        if (!isAlive)
        {
            gameObject.SetActive(false);
        }*/
    }

    //Moves cloud in the given direction for duration seconds
    public IEnumerator MoveCloud(Vector3 direction, float duration)
    {
        Vector3 startingPos = transform.position;
        Vector3 finalPos = transform.position + (direction * 10);
        AudioPlayer.main.PlaySound(cloudMove);
        yield return new WaitForSeconds(1f);
        while (timer < duration)
        {
            transform.position = Vector3.Lerp(startingPos, finalPos, (timer / duration));
            timer += Time.deltaTime;
            yield return null;
        }
        while (timer < duration + 2)
        {
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, (timer - duration) / 2f);
            timer += Time.deltaTime;
            yield return null;
        }
        isAlive = false;
        
            
    }
    
    //Functions for the original sphere method for cloud rendering
    private void AddBall()
    {
        GameObject newBall = Instantiate(spherePrefab);
        newBall.transform.localScale = Vector3.zero;
        newBall.transform.parent = transform;
        newBall.transform.localPosition = Random.onUnitSphere * Random.Range(0, maxBallRange);
        float maxRadius = Random.Range(maxSphereRadiusRange.x, maxSphereRadiusRange.y);
        CloudBall firstBall = new CloudBall(newBall, maxRadius);
        spheres.Add(firstBall);
    }
    private void UpdateBalls()
    {
        for (int i = 0; i < spheres.Count; i++)
        {
            CloudBall currentBall = spheres[i];
            currentBall.lifeTimer += Time.deltaTime * speedMultiplier;
            if (currentBall.lifeTimer > 2 * currentBall.maxRadius)
            {
                Destroy(currentBall.instance.gameObject);
                spheres.RemoveAt(i);
                AddBall();
                return;
            }

            float c = Mathf.PI / (2 * currentBall.maxRadius);
            currentBall.instance.transform.localScale = Vector3.one * currentBall.maxRadius * Mathf.Sin(c * currentBall.lifeTimer);
            spheres[i] = currentBall;
        }
    }
    public struct CloudBall
    {
        public float lifeTimer;
        public float maxRadius;
        public GameObject instance;

        public CloudBall(GameObject instance, float maxRadius)
        {
            lifeTimer = 0;
            this.maxRadius = maxRadius;
            this.instance = instance;
        }
    }
}
