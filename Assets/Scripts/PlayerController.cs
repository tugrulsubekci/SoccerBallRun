using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private Transform playerTrans;
    [SerializeField] float forwardVelocity = 3;

    private float xRange = 3.5f;
    private float duration = 0.5f;

    [SerializeField] GameObject Goal;
    [SerializeField] float shotPower;
    private float shotPosZ;
    [SerializeField] float distanceFromBall = 15;
    private float scaleMultiplier = 1.25f;
    public bool isPositionOkay;
    public bool isShooted;
    private bool isTriggeredSoon;

    private bool isCameraPositionOkay;
    [SerializeField] GameObject tapToShootText;
    private GameObject menu;
    private GameObject revive;

    private GameManager gameManager;
    private AudioManager audioManager;
    private Camera mainCamera;
    private Rolling rolling;

    private Vector3 revivePos;
    private Vector3 reviveOffset = new Vector3(0, 0, -10);

    private PhysicMaterial bouncyMaterial;
    [SerializeField] ParticleSystem explosionParticle;

    //new movement mechanic
    private Vector3 newPos;
    private float xPos;
    private float _moveFactorX;
    private float _lastFrameFingerPositionX;
    private int screenWidth;

    // for shoot
    private SphereCollider sphereColl;
    private BoxCollider boxColl;
    private Rigidbody goalPostRb;
    private Rigidbody goalRb;


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
        sphereColl = GetComponent<SphereCollider>();
        boxColl = GetComponent<BoxCollider>();
        bouncyMaterial = boxColl.material;
        goalPostRb = Goal.GetComponent<Rigidbody>();
        goalRb = Goal.transform.GetChild(2).GetComponent<Rigidbody>();
        screenWidth = Screen.width;
        if (OldAdManager.Instance.isRevived)
        {
            Revive();
        }
    }

    private void Update()
    {
        if (gameManager.isGameStarted)
        {
            GetFingerPos();
        }
        if (isPositionOkay && !isShooted)
        {
            HandleShoot();
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
    void MovePlayer()
    {
        if (gameManager.isGameStarted)
        {
            xPos = _moveFactorX * xRange * 2 / screenWidth;

            newPos = new Vector3(playerTrans.position.x + xPos, playerTrans.position.y, playerTrans.position.z + Time.fixedDeltaTime * forwardVelocity);
            if (playerTrans.position.x + xPos > xRange)
            {
                newPos.x = xRange;
            }
            else if (playerTrans.position.x + xPos < -xRange)
            {
                newPos.x = -xRange;
            }

            playerRigidbody.MovePosition(newPos);
        }
    }
    private void GetFingerPos()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _lastFrameFingerPositionX = Input.mousePosition.x;
            CheckFingerPos();
        }
        else if (Input.GetMouseButton(0))
        {
            _moveFactorX = Input.mousePosition.x - _lastFrameFingerPositionX;
            _lastFrameFingerPositionX = Input.mousePosition.x;
            CheckFingerPos();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _moveFactorX = 0f;
        }
    }
    private void CheckFingerPos()
    {
        if (_lastFrameFingerPositionX > screenWidth)
        {
            _lastFrameFingerPositionX = screenWidth;
        }
        else if (_lastFrameFingerPositionX < 0)
        {
            _lastFrameFingerPositionX = 0;
        }
    }

    private void HandleShoot()
    {
        if (playerRigidbody.collisionDetectionMode != CollisionDetectionMode.Continuous)
        {
            playerTrans.rotation = Quaternion.Euler(Vector3.zero);
            playerRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            boxColl.enabled = true;
            sphereColl.enabled = false;
            goalPostRb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            goalRb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SmallBall") && !isTriggeredSoon)
        {
            audioManager.Play("SmallBall");
            // playerTrans.localScale = playerTrans.localScale / scaleMultiplier;
            playerTrans.DOScale(playerTrans.localScale / scaleMultiplier, duration).SetEase(Ease.InOutBack);
            other.gameObject.GetComponent<Panel>().ScaleX();
            isTriggeredSoon = true;
            Invoke(nameof(Trigger), 1);
        }
        if (other.CompareTag("BigBall") && !isTriggeredSoon)
        {
            audioManager.Play("BigBall");
            // playerTrans.localScale = playerTrans.localScale * scaleMultiplier;
            playerTrans.DOScale(playerTrans.localScale * scaleMultiplier, duration).SetEase(Ease.InOutBack);
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
        if (revivePos.z < shotPosZ)
        {
            playerRigidbody.MovePosition(revivePos);
        }
        else if (revivePos.z >= shotPosZ)
        {
            revivePos.z = shotPosZ - 10;
            playerRigidbody.MovePosition(revivePos);
        }
        playerTrans.localScale = OldAdManager.Instance.playerScale;
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