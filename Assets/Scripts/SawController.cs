using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawController : MonoBehaviour
{
    private bool isRight = true;
    private bool isLeft = false;

    [SerializeField] float moveSpeed = 2;
    // Update is called once per frame
    void Update()
    {
        if (isRight)
        {
            gameObject.transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
        }
        if (isLeft)
        {
            gameObject.transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        }
        if(transform.position.x <= -3.5)
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
