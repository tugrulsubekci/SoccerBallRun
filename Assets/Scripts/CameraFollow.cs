using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 newPos = new Vector3(0, 6.75f, -12.0f);
    private float offsetZ = 12.0f;
    private PlayerController playerController;
    private Transform cameraTransform;
    private Transform playerTransform;

    [SerializeField] GameObject player;
    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        playerTransform = player.transform;
        cameraTransform = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        if (!playerController.isShooted)
        {
            newPos.z = playerTransform.position.z - offsetZ;
            cameraTransform.localPosition = newPos;
        }
    }
}
