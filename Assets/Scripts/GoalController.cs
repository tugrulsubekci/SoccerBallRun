using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    private bool isRight = true;
    private bool isLeft = false;

    [SerializeField] float moveSpeed = 2;

    private GameManager gameManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if(gameManager.isGameStarted)
        {
            Move();
        }
    }
    private void Move()
    {
        if (isRight)
        {
            gameObject.transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);
        }
        if (isLeft)
        {
            gameObject.transform.Translate(moveSpeed * Time.deltaTime * Vector3.back);
        }
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
