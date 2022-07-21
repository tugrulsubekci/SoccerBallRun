using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isGameStarted;
    [SerializeField] GameObject instructionalCanvas;
    [SerializeField] GameObject menuCanvas;
    private void Update()
    {
        if(isGameStarted && Input.GetMouseButtonDown(0))
        {
            instructionalCanvas.SetActive(false);
        }
    }
    public void StartGame()
    {
        isGameStarted = true;
        instructionalCanvas.SetActive(true);
        menuCanvas.SetActive(false);
    }
}
