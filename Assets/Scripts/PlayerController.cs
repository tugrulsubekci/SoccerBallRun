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
        if (AdsInitializer.Instance.isRevived)
        {
            Revive();
        }
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
            playerRigidbody.constraints = RigidbodyConstraints.FreezePositionY;
            // Keyboard shoot
            /*if (Input.GetKeyDown(KeyCode.Space) && isCameraPositionOkay)
            {   
                isShooted = true;
                playerRigidbody.AddForce(Vector3.forward * shotPower, ForceMode.Impulse);
                StartCoroutine(CheckShoot());
            }*/
            if (Input.GetMouseButtonDown(0) && isCameraPositionOkay)
            {
                FindObjectOfType<AudioManager>().Play("Shoot");
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
        else if(!gameManager.isGameStarted)
        {
            playerRigidbody.velocity = Vector3.zero;
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
            FindObjectOfType<AudioManager>().Play("SmallBall");
            transform.localScale = transform.localScale / 1.2f;
            other.gameObject.GetComponent<Animator>().SetBool("isDestroyed", true);
            Destroy(other.gameObject, 0.6f);
        }
        if (other.CompareTag("BigBall"))
        {
            FindObjectOfType<AudioManager>().Play("BigBall");
            transform.localScale = transform.localScale * 1.2f;
            other.gameObject.GetComponent<Animator>().SetBool("isDestroyed", true);
            Destroy(other.gameObject, 0.6f);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            FindObjectOfType<AudioManager>().Play("BallExplosion");
            gameObject.SetActive(false);
            Instantiate(explosionParticle, transform.position, transform.rotation);
            gameManager.GameOver();
        }
        else if (collision.gameObject.CompareTag("Goal") && !gameManager.isLevelCompleted)
        {
            gameManager.LevelCompleted();

        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            FindObjectOfType<AudioManager>().Play("Bounce");
        }
    }
    private void Revive()
    {
        GameObject[] x = GameObject.FindGameObjectsWithTag("Obstacle");
        GameObject[] y = GameObject.FindGameObjectsWithTag("SmallBall");
        GameObject[] z = GameObject.FindGameObjectsWithTag("BigBall");
        GameObject[] c = GameObject.FindGameObjectsWithTag("Coin");
        foreach (GameObject obstacle in x)
        {
            if (obstacle.transform.position.z <= AdsInitializer.Instance.playerPos.z)
            {
                Destroy(obstacle);
            }
        }
        foreach (GameObject obstacle in y)
        {
            if (obstacle.transform.position.z <= AdsInitializer.Instance.playerPos.z)
            {
                Destroy(obstacle);
            }
        }
        foreach (GameObject obstacle in z)
        {
            if (obstacle.transform.position.z <= AdsInitializer.Instance.playerPos.z)
            {
                Destroy(obstacle);
            }
        }
        foreach (GameObject obstacle in c)
        {
            if (obstacle.transform.position.z <= AdsInitializer.Instance.playerPos.z)
            {
                Destroy(obstacle);
            }
        }
        Vector3 revivePos = AdsInitializer.Instance.playerPos + new Vector3(0, 0, -10);
        if(revivePos.z < shotPosZ)
        {
            transform.position = revivePos;
        }
        else if(revivePos.z >= shotPosZ)
        {
            revivePos.z = shotPosZ - 10;
            transform.position = revivePos;
        }
        gameManager._coins = AdsInitializer.Instance.reviveCoins;
        AdsInitializer.Instance.isRevived = false;
    }
    IEnumerator CheckShoot()
    {
        yield return new WaitForSeconds(2);
        while(!gameManager.gameOver && !gameManager.isLevelCompleted)
        {
            gameManager.GameOver();
        }
    }
    IEnumerator CameraPosition()
    {
        yield return new WaitForSeconds(2);
        while (!isCameraPositionOkay)
        {
            isCameraPositionOkay = true;
            tapToShootText.SetActive(true);
        }
    }
}