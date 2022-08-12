using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 6.75f, -9.0f);
    // private Vector3 offset2 = new Vector3(0, 2.4f, -3.0f);
    // private Vector3 offset3 = new Vector3(0, -3, 0);
    private Vector3 desiredPosition;
    private Vector3 smoothedPosition;
    // private Vector3 newPosition;

    private float smoothSpeed = 0.125f;
    // private float smoothSpeed2 = 0.05f;

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
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!playerController.isShooted && player.gameObject != null)
        {
            desiredPosition = offset + playerTransform.position;
            smoothedPosition = Vector3.Lerp(cameraTransform.position, desiredPosition, smoothSpeed);
            cameraTransform.position = smoothedPosition;

            /*if (playerController.isPositionOkay)
            {
                offset = Vector3.Lerp(offset, offset2, smoothSpeed2);
            }
            else
            {
                newPosition = playerTransform.position - offset3;
                cameraTransform.LookAt(newPosition);
            }*/
        }
    }
}
