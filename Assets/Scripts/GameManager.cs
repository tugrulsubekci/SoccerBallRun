using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isGameStarted;
    [SerializeField] GameObject instructionalObjects;
    [SerializeField] GameObject menuObjects;
    private void Update()
    {
        if(isGameStarted && Input.GetMouseButtonDown(0))
        {
            instructionalObjects.SetActive(false);
        }
    }
    public void StartGame()
    {
        isGameStarted = true;
        instructionalObjects.SetActive(true);
        menuObjects.SetActive(false);
    }
}
