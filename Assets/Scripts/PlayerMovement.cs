using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 0.1f;
    public float jumpStrength = 5f;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputs = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        rb.MovePosition(transform.position + new Vector3(inputs.x, 0, inputs.y) * speed);    

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        }
    }
}
