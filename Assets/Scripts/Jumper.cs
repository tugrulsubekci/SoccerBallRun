using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] float jumpForce = 13;
    private Rigidbody playerRb;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<AudioManager>().Play("Jump");
            playerRb = other.gameObject.GetComponent<Rigidbody>();
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
