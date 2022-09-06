using UnityEngine;

public class GoalController : MonoBehaviour
{
    private bool isRight = true;
    private bool isLeft = false;

    [SerializeField] float moveSpeed = 2;
    private float xBound = 3.2f;

    private PlayerController playerController;
    private Rigidbody goRigidbody;
    private Transform goTrans;
    private Vector3 deltaPos;
    private void Awake()
    {
        deltaPos = new Vector3(moveSpeed * Time.fixedDeltaTime, 0, 0);
        goRigidbody = gameObject.GetComponent<Rigidbody>();
        goTrans = transform;
        playerController = FindObjectOfType<PlayerController>();
    }

    void FixedUpdate()
    {
        if (playerController.isPositionOkay)
        {
            Move();
        }
    }

    private void Move()
    {
        if (isRight)
        {
            goRigidbody.MovePosition(goTrans.position - deltaPos);
        }
        else
        {
            goRigidbody.MovePosition(goTrans.position + deltaPos);
        }
        CheckPosition();
    }

    private void CheckPosition()
    {
        if (goTrans.position.x <= -xBound)
        {
            isLeft = true;
            isRight = false;
        }
        if (goTrans.position.x >= xBound)
        {
            isLeft = false;
            isRight = true;
        }
    }
}
