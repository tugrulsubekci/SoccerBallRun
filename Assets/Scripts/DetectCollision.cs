using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    public GameObject coinParticle;
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        Instantiate(coinParticle, transform.position, transform.rotation);
    }
}
