using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private float forwardVelocity = 4;

    private float horizontalInput;
    private float movementSpeed = 5;

    private float xRange = 4;

    // Start is called before the first frame update
    void Start()
    { 
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
        CheckPosition();
    }

    void MovePlayer()
    {
        horizontalInput = Input.GetAxis("Horizontal") * movementSpeed;
        playerRigidbody.velocity = new Vector3(horizontalInput, -0.1f , forwardVelocity);
    }

    void CheckPosition()
    {
        if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SmallBall"))
        {
            transform.localScale = transform.localScale * 0.8f;
            Destroy(other.gameObject);
        }
        if (other.CompareTag("BigBall"))
        {
            transform.localScale = transform.localScale * 1.2f;
            Destroy(other.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            playerRigidbody.useGravity = false;
        }
    }
}
