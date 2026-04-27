using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    public float speed = 5f;
    public float jumpStrength = 5f;
    private Rigidbody rb;
    [SerializeField] private Transform sprite;
    public Vector2 prevInput = Vector2.zero;
    private Transform playerSpawn;
    public bool canMove = true;
    void Start()
    {
        playerSpawn = NetManager.main.playerSpawn;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (photonView.IsMine && canMove)
        {
            InputMovement();
        }
    }

    private void InputMovement()
    {
        Vector2 inputs = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        

        float targetRot = prevInput.x < 0 ? 180 : 0; //0 or 180 depending on previous input
        if (inputs.x < 0)
        {
            targetRot = inputs.x * 180;
        }
        
        sprite.localEulerAngles = new Vector3(transform.localEulerAngles.x, targetRot, transform.localEulerAngles.z);

        rb.MovePosition(transform.position + new Vector3(inputs.x, 0, inputs.y) * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (inputs.x != 0)
        {
            prevInput = inputs;
        }
        if (transform.position.y < -17)
        {
            transform.position = playerSpawn.position;
        }
    }
    private void Jump()
    {
        Debug.Log(Physics.Raycast(transform.position, Vector3.down, 2f));
        Debug.DrawRay(transform.position, Vector3.down, Color.red, 2f);
        if (Physics.Raycast(transform.position, Vector3.down, 2f))
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
            
        }
        
    }

    
    
    
}
