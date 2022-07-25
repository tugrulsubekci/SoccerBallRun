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
    public int _coins;

    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject levelCompletedPanel;
    [SerializeField] ParticleSystem[] confeties;
    private void Start()
    {
        DataManager.Instance.Load();
        coinText.text = DataManager.Instance.coins.ToString();
    }
    public void AddCoin(int amount)
    {
        _coins += amount;
        coinText.text = (DataManager.Instance.coins + _coins).ToString();
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
        DataManager.Instance.coins += _coins;
        DataManager.Instance.Save();
        for (int i = 0; i < confeties.Length; i++)
        {
            confeties[i].Play();
        }
    }
}
