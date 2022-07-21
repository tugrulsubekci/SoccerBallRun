using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipAnimationController : MonoBehaviour
{
    private GameManager gameManager;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.isGameStarted)
        {
            animator.SetBool("isGameStarted", true);
        }
    }
}
