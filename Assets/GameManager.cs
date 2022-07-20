using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isGameStarted;
    [SerializeField] GameObject menuCanvas;
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isGameStarted = true;
        }
        if(isGameStarted)
        {
            menuCanvas.SetActive(false);
        }
    }
}
