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

    [SerializeField] GameObject Goal;
    [SerializeField] float shotPower;
    private float shotPosZ;
    public bool isPositionOkay;
    public bool isShooted;
    // Start is called before the first frame update
    void Start()
    { 
        playerRigidbody = GetComponent<Rigidbody>();
        shotPosZ = Goal.transform.position.z - 15;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.position.z < shotPosZ)
        {
            MovePlayer();
        }
        else if (transform.position.z >= shotPosZ)
        {
            if(!isShooted)
            {
                playerRigidbody.velocity = Vector3.zero;
                isPositionOkay = true;
            }
        }
        if(isPositionOkay)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                playerRigidbody.AddForce(Vector3.forward * shotPower, ForceMode.Impulse);
                isShooted = true;
            }
        }
        CheckPosition();
    }

    void MovePlayer()
    {
            horizontalInput = Input.GetAxis("Horizontal") * movementSpeed;
            playerRigidbody.velocity = new Vector3(horizontalInput, playerRigidbody.velocity.y , forwardVelocity);
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
}
