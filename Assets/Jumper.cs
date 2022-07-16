using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float jumpForce = 13;
            Rigidbody playerRigidboy = other.gameObject.GetComponent<Rigidbody>();
            playerRigidboy.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
