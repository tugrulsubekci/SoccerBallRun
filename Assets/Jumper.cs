using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] float jumpForce = 13;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRigidboy = other.gameObject.GetComponent<Rigidbody>();
            playerRigidboy.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
