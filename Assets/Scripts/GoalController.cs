using UnityEngine;

public class GoalController : MonoBehaviour
{
    private bool isRight = true;
    private bool isLeft = false;

    [SerializeField] float moveSpeed = 2;

    private PlayerController playerController;
    private Transform goTransform;
    private void Start()
    {
        goTransform = gameObject.transform;
        playerController = FindObjectOfType<PlayerController>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(playerController.isPositionOkay)
        {
            Move();
        }
    }
    private void Move()
    {
        if (isRight)
        {
            goTransform.Translate(moveSpeed * Time.fixedDeltaTime * Vector3.forward);
        }
        else
        {
            goTransform.Translate(moveSpeed * Time.fixedDeltaTime * Vector3.back);
        }
        CheckPosition();
    }
    private void CheckPosition()
    {
        if (transform.position.x <= -3.5)
        {
            isLeft = true;
            isRight = false;
        }
        if (transform.position.x >= 3.5)
        {
            isLeft = false;
            isRight = true;
        }
    }
}
