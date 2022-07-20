using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    [SerializeField] float forwardVelocity = 4;

    public float horizontalInput;
    [SerializeField] float movementSpeed = 5;

    private float xRange = 4;

    [SerializeField] GameObject Goal;
    [SerializeField] float shotPower;
    private float shotPosZ;
    [SerializeField] float distanceFromBall = 15;
    public bool isPositionOkay;
    public bool isShooted;

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        shotPosZ = Goal.transform.position.z - distanceFromBall;
    }

    // Update is called once per frame
    /*void FixedUpdate()
    {
        if (transform.position.z < shotPosZ)
        {
            MoveePlayer();
        }
        else if (transform.position.z >= shotPosZ)
        {
            if (!isShooted)
            {
                playerRigidbody.velocity = Vector3.zero;
                isPositionOkay = true;
            }
        }

        CheckPosition();
    }*/

    private void Update()
    {
        if (transform.position.z < shotPosZ)
        {
            MoveePlayer();
        }
        else if (transform.position.z >= shotPosZ)
        {
            if (!isShooted)
            {
                playerRigidbody.velocity = Vector3.zero;
                isPositionOkay = true;
            }
        }

        CheckPosition();
        Debug.Log("Mouse Position: " + Input.mousePosition.x);
        Debug.Log("Ball Position: " + Camera.main.WorldToScreenPoint(transform.position).x);
        if (isPositionOkay && !isShooted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerRigidbody.AddForce(Vector3.forward * shotPower, ForceMode.Impulse);
                isShooted = true;
            }
        }
    }

    void MovePlayer()
    {
        if(gameManager.isGameStarted)
        {
            horizontalInput = Input.GetAxis("Horizontal") * movementSpeed;
            playerRigidbody.velocity = new Vector3(horizontalInput, playerRigidbody.velocity.y, forwardVelocity);
        }

    }
    void MoveePlayer()
    {
        if (gameManager.isGameStarted)
        {
            if(Input.GetMouseButton(0))
            {
                if(Input.mousePosition.x > Camera.main.WorldToScreenPoint(transform.position).x)
                {
                    if(horizontalInput < 5)
                    {
                        horizontalInput += 1 * Time.deltaTime * movementSpeed;
                    }
                    
                }
                else if (Input.mousePosition.x < Camera.main.WorldToScreenPoint(transform.position).x)
                {
                    if(horizontalInput > -5)
                    {
                        horizontalInput -= 1 * Time.deltaTime * movementSpeed;
                    }
                    
                }
                playerRigidbody.velocity = new Vector3(horizontalInput, playerRigidbody.velocity.y, forwardVelocity);
            }
            else
            {
                horizontalInput = 0;
                playerRigidbody.velocity = new Vector3(horizontalInput, playerRigidbody.velocity.y, forwardVelocity);
            }
        }
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
            other.gameObject.GetComponent<PanelAnimation>().isDestroyed = true;
            Destroy(other.gameObject, 0.6f);
        }
        if (other.CompareTag("BigBall"))
        {
            transform.localScale = transform.localScale * 1.2f;
            other.gameObject.GetComponent<PanelAnimation>().isDestroyed = true;
            Destroy(other.gameObject, 0.6f);
        }
    }
}
