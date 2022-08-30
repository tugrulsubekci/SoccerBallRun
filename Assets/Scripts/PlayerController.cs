using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private Transform playerTrans;
    [SerializeField] float forwardVelocity = 3;

    public float horizontalInput;
    [SerializeField] float movementSpeed = 5;
    private float rotationSpeed = 360;

    private float xRange = 4;

    [SerializeField] GameObject Goal;
    [SerializeField] float shotPower;
    private float shotPosZ;
    [SerializeField] float distanceFromBall = 15;
    public bool isPositionOkay;
    public bool isShooted;

    private bool isCameraPositionOkay;
    [SerializeField] GameObject tapToShootText;
    private GameObject menu;
    private GameObject revive;

    private GameManager gameManager;
    private AudioManager audioManager;
    private Camera mainCamera;
    private float _direction;
    private float right = 1;
    private float left = -1;
    private float zero = 0;
    private Vector3 deltaPosition;
    private Quaternion deltaRotation;
    private Vector3 revivePos;
    private Vector3 reviveOffset = new Vector3(0, 0, -10);
    private PhysicMaterial bouncyMaterial;
    [SerializeField] ParticleSystem explosionParticle;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerTrans = GetComponent<Transform>();
        mainCamera = Camera.main;
        shotPosZ = Goal.transform.position.z - distanceFromBall;
        menu = tapToShootText.transform.parent.GetChild(0).gameObject;
        revive = tapToShootText.transform.parent.GetChild(7).gameObject;
        deltaPosition = new Vector3(0,0,forwardVelocity);
        deltaRotation = Quaternion.Euler(rotationSpeed * Time.fixedDeltaTime * Vector3.right);
        bouncyMaterial = gameObject.GetComponent<SphereCollider>().material;
        if (OldAdManager.Instance.isRevived)
        {
            Revive();
        }
    }

    void FixedUpdate()
    {
        if (playerTrans.position.z < shotPosZ)
        {
            MovePlayer();
        }
        else if (playerTrans.position.z >= shotPosZ)
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
            if (Input.mousePosition.x > mainCamera.WorldToScreenPoint(playerTrans.position).x)
            {
                _direction = right;
            }
            else if (Input.mousePosition.x < mainCamera.WorldToScreenPoint(playerTrans.position).x)
            {
                _direction = left;
            }
            else
            {
                _direction = zero;
            }
        }
        else
        {
            _direction = zero;
        }

        if (isPositionOkay && !isShooted)
        {
            if(playerRigidbody.collisionDetectionMode != CollisionDetectionMode.ContinuousDynamic)
            {
                playerRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }
            // Keyboard shoot
            /*if (Input.GetKeyDown(KeyCode.Space) && isCameraPositionOkay)
            {   
                isShooted = true;
                playerRigidbody.AddForce(Vector3.forward * shotPower, ForceMode.Impulse);
                StartCoroutine(CheckShoot());
            }*/
            if (Input.GetMouseButtonDown(0) && isCameraPositionOkay)
            {
                bouncyMaterial.bounciness = 0;
                audioManager.Play("Shoot");
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
            //Move Ball
            horizontalInput = _direction * movementSpeed;
            deltaPosition.x = horizontalInput;
            playerRigidbody.MovePosition(playerTrans.position + Time.fixedDeltaTime * deltaPosition);

            //playerRigidbody.velocity = new Vector3(horizontalInput, playerRigidbody.velocity.y, forwardVelocity);

            // Roll ball
            playerRigidbody.MoveRotation(playerRigidbody.rotation * deltaRotation);
        }
        else if(!gameManager.isGameStarted)
        {
            playerRigidbody.velocity = Vector3.zero;
        }
    }

    void CheckPosition()
    {
        if (playerTrans.position.x > xRange)
        {
            playerTrans.position = new Vector3(xRange, playerTrans.position.y, playerTrans.position.z);
        }
        if (playerTrans.position.x < -xRange)
        {
            playerTrans.position = new Vector3(-xRange, playerTrans.position.y, playerTrans.position.z);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SmallBall"))
        {
            audioManager.Play("SmallBall");
            transform.localScale = transform.localScale / 1.2f;
            other.gameObject.GetComponent<Animator>().SetBool("isDestroyed", true);
            Destroy(other.gameObject, 0.6f);
        }
        if (other.CompareTag("BigBall"))
        {
            audioManager.Play("BigBall");
            transform.localScale = transform.localScale * 1.2f;
            other.gameObject.GetComponent<Animator>().SetBool("isDestroyed", true);
            Destroy(other.gameObject, 0.6f);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            audioManager.Play("BallExplosion");
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
            audioManager.Play("Bounce");
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
            if (obstacle.transform.position.z <= OldAdManager.Instance.playerPos.z)
            {
                obstacle.SetActive(false);
            }
        }
        foreach (GameObject obstacle in y)
        {
            if (obstacle.transform.position.z <= OldAdManager.Instance.playerPos.z)
            {
                obstacle.SetActive(false);
            }
        }
        foreach (GameObject obstacle in z)
        {
            if (obstacle.transform.position.z <= OldAdManager.Instance.playerPos.z)
            {
                obstacle.SetActive(false);
            }
        }
        foreach (GameObject obstacle in c)
        {
            if (obstacle.transform.position.z <= OldAdManager.Instance.playerPos.z)
            {
                obstacle.SetActive(false);
            }
        }
        revivePos = OldAdManager.Instance.playerPos + reviveOffset;
        if(revivePos.z < shotPosZ)
        {
            playerRigidbody.MovePosition(revivePos);
        }
        else if(revivePos.z >= shotPosZ)
        {
            revivePos.z = shotPosZ - 10;
            playerRigidbody.MovePosition(revivePos);
        }
        OldAdManager.Instance.reviveCoins = 0;
        OldAdManager.Instance.isRevived = false;
        menu.SetActive(false);
        revive.SetActive(true);
        gameManager.AddCoin(OldAdManager.Instance.reviveCoins);
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