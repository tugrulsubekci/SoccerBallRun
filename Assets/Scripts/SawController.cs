using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawController : MonoBehaviour
{
    private GameManager gameManager;
    private Animator animator;
    private void Start()
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