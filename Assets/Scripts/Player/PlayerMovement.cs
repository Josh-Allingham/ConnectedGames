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
   
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            InputMovement();
        }
    }

    private void InputMovement()
    {
        Vector2 inputs = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        rb.MovePosition(transform.position + new Vector3(inputs.x, 0, inputs.y) * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }
    private void Jump()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 1f))
        {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        }
        
    }

    
    
    
}
