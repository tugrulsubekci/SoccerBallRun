using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 6.75f, -9.0f);
    private Vector3 offset2 = new Vector3(0, 2.4f, -3.0f);
    private PlayerController playerController;
    [SerializeField] GameObject player;
    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (!playerController.isShooted)
        {
            transform.position = offset + player.transform.position;
            if (playerController.isPositionOkay)
            {
                offset = Vector3.Lerp(offset, offset2, 0.005f);
            }
        }
    }
}
