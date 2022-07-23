using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public bool isGameStarted;
    public bool gameOver;
    public bool isLevelCompleted;
    public int ballIndex;
    public int coins = 1000;

    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject levelCompletedPanel;
    public void AddCoin(int amount)
    {
        coins += amount;
        coinText.text = coins.ToString();
    }
    public void GameOver()
    {
        gameOver = true;
        gameOverPanel.SetActive(true);
    }

    public void LevelCompleted()
    {
        isLevelCompleted = true;
        levelCompletedPanel.SetActive(true);
    }
}
