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

    private bool isCameraPositionOkay;
    [SerializeField] GameObject tapToShootText;

    private GameManager gameManager;
    private Vector3 _direction;

    [SerializeField] ParticleSystem explosionParticle;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        shotPosZ = Goal.transform.position.z - distanceFromBall;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.z < shotPosZ)
        {
            MovePlayer();
        }
        else if (transform.position.z >= shotPosZ)
        {
            StartCoroutine(CameraPosition());
            if (!isShooted)
            {
                playerRigidbody.velocity = Vector3.zero;
                isPositionOkay = true;
            }
        }

        CheckPosition();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Input.mousePosition.x > Camera.main.WorldToScreenPoint(transform.position).x)
            {
                _direction = Vector3.right;
            }
            else if (Input.mousePosition.x < Camera.main.WorldToScreenPoint(transform.position).x)
            {
                _direction = Vector3.left;
            }
            else
            {
                _direction = Vector3.zero;
            }
        }
        else
        {
            _direction = Vector3.zero;
        }

        if (isPositionOkay && !isShooted)
        {
            // Keyboard shoot
            /*if (Input.GetKeyDown(KeyCode.Space) && isCameraPositionOkay)
            {   
                isShooted = true;
                playerRigidbody.AddForce(Vector3.forward * shotPower, ForceMode.Impulse);
                StartCoroutine(CheckShoot());
            }*/
            if (Input.GetMouseButtonDown(0) && isCameraPositionOkay)
            {
                playerRigidbody.AddForce(Vector3.forward * shotPower, ForceMode.Impulse);
                isShooted = true;
                StartCoroutine(CheckShoot());
            }
        }
    }
    // Keyboard movement
    /*void MovePlayer()
    {
        if (gameManager.isGameStarted)
        {
            horizontalInput = Input.GetAxis("Horizontal") * movementSpeed;
            playerRigidbody.velocity = new Vector3(horizontalInput, playerRigidbody.velocity.y, forwardVelocity);
        }

    }*/
    void MovePlayer()
    {
        if (gameManager.isGameStarted)
        {
            horizontalInput = _direction.x * movementSpeed;
            playerRigidbody.velocity = new Vector3(horizontalInput, playerRigidbody.velocity.y, forwardVelocity);
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
            transform.localScale = transform.localScale / 1.2f;
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
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
            Instantiate(explosionParticle,transform.position, transform.rotation);
            gameManager.GameOver();
        }
        else if(collision.gameObject.CompareTag("Goal") && !gameManager.isLevelCompleted)
        {
            gameManager.LevelCompleted();

        }
    }
    IEnumerator CheckShoot()
    {
        yield return new WaitForSeconds(2);
        if(!gameManager.isLevelCompleted)
        {
            gameManager.GameOver();
        }
    }
    IEnumerator CameraPosition()
    {
        yield return new WaitForSeconds(2);
        isCameraPositionOkay = true;
        tapToShootText.SetActive(true);
    }
}