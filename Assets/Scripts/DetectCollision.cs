using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    public GameObject coinParticle;
    private GameManager gameManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        Instantiate(coinParticle, transform.position, transform.rotation);
        gameManager.AddCoin(1);
    }
}
