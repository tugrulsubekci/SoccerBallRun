using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public bool isGameStarted;
    public bool gameOver;
    public bool isLevelCompleted;
    public int _coins;

    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject levelCompletedPanel;
    [SerializeField] ParticleSystem[] confeties;

    private void Start()
    {
        coinText.text = DataManager.Instance.coins.ToString();
        levelText.text = $"LEVEL {DataManager.Instance.levelIndex + 1}";
    }
    public void AddCoin(int amount)
    {
        _coins += amount;
        coinText.text = (DataManager.Instance.coins + _coins).ToString();
    }
    public void BuyItem(int price)
    {
        DataManager.Instance.coins -= price;
        coinText.text = DataManager.Instance.coins.ToString();
    }
    public bool HasEnoughCoins(int price)
    {
        return DataManager.Instance.coins >= price;
    }
    public void GameOver()
    {
        FindObjectOfType<AudioManager>().Play("GameOver");
        gameOver = true;
        gameOverPanel.SetActive(true);
    }

    public void LevelCompleted()
    {
        FindObjectOfType<AudioManager>().Play("LevelCompleted");
        FindObjectOfType<AudioManager>().Play("Confetti");
        isLevelCompleted = true;
        levelCompletedPanel.SetActive(true);
        DataManager.Instance.coins += _coins;
        DataManager.Instance.levelIndex++;
        DataManager.Instance.Save();
        for (int i = 0; i < confeties.Length; i++)
        {
            confeties[i].Play();
        }
    }
}
