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
   
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (photonView.IsMine)
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
    }
    private void Jump()
    {
        if (Physics.Raycast(transform.position, Vector3.down, .5f))
        {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        }
        
    }

    
    
    
}
