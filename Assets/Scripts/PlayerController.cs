using System.Collections;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private Transform playerTrans;
    [SerializeField] float forwardVelocity = 3;

    public float horizontalInput;
    [SerializeField] float movementSpeed = 5;

    private float xRange = 4;

    [SerializeField] GameObject Goal;
    [SerializeField] float shotPower;
    private float shotPosZ;
    [SerializeField] float distanceFromBall = 15;
    public bool isPositionOkay;
    public bool isShooted;
    private bool isTriggeredSoon;
    private bool outOfBounds;

    private bool isCameraPositionOkay;
    [SerializeField] GameObject tapToShootText;
    private GameObject menu;
    private GameObject revive;

    private GameManager gameManager;
    private AudioManager audioManager;
    private Camera mainCamera;
    private Rolling rolling;
    private float _direction;
    private float right = 1;
    private float left = -1;
    private float zero = 0;
    private Vector3 deltaPosition;
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
        rolling = GetComponent<Rolling>();
        mainCamera = Camera.main;
        shotPosZ = Goal.transform.position.z - distanceFromBall;
        menu = tapToShootText.transform.parent.GetChild(0).gameObject;
        revive = tapToShootText.transform.parent.GetChild(7).gameObject;
        deltaPosition = new Vector3(0,0,forwardVelocity);
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
        else if (playerTrans.position.z >= shotPosZ && !isPositionOkay)
        {
            StartCoroutine(CameraPosition());
            rolling.Stop();
            if (!isShooted)
            {
                playerRigidbody.velocity = Vector3.zero;
                isPositionOkay = true;
            }
        }

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
    void MovePlayer()
    {
        if (gameManager.isGameStarted)
        {
            //Move Ball
            horizontalInput = _direction * movementSpeed;
            if(playerTrans.position.x >= xRange && _direction > 0)
            {
                deltaPosition.x = zero;
            }
            else if (playerTrans.position.x <= -xRange && _direction < 0)
            {
                deltaPosition.x = zero;
            }
            else
            {
                deltaPosition.x = horizontalInput;
            }
            playerRigidbody.MovePosition(playerTrans.position + Time.fixedDeltaTime * deltaPosition);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SmallBall") && !isTriggeredSoon)
        {
            audioManager.Play("SmallBall");
            playerTrans.localScale = playerTrans.localScale / 1.2f;
            other.gameObject.GetComponent<Panel>().ScaleX();
            isTriggeredSoon = true;
            Invoke(nameof(Trigger), 1);
        }
        if (other.CompareTag("BigBall") && !isTriggeredSoon)
        {
            audioManager.Play("BigBall");
            playerTrans.localScale = playerTrans.localScale * 1.2f;
            other.gameObject.GetComponent<Panel>().ScaleX();
            isTriggeredSoon = true;
            Invoke(nameof(Trigger), 1);
        }
    }

    private void Trigger()
    {
        isTriggeredSoon = false;
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
        gameManager.AddCoin(OldAdManager.Instance.reviveCoins);
        OldAdManager.Instance.reviveCoins = 0;
        OldAdManager.Instance.isRevived = false;
        menu.SetActive(false);
        revive.SetActive(true);
    }
    IEnumerator CheckShoot()
    {
        yield return new WaitForSeconds(2);
        while (!gameManager.gameOver && !gameManager.isLevelCompleted)
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