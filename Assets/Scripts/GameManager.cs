using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
        // DataManager.Instance.Load();
        /*if(DataManager.Instance.levelIndex != SceneManager.GetActiveScene().buildIndex)
        {
            SceneManager.LoadScene(DataManager.Instance.levelIndex);
        }*/
        coinText.text = DataManager.Instance.coins.ToString();
        levelText.text = $"LEVEL {DataManager.Instance.levelIndex + 1}";
    }
    public void AddCoin(int amount)
    {
        _coins += amount;
        coinText.text = (DataManager.Instance.coins + _coins).ToString();
    }
    public void BuyBall1()
    {
        DataManager.Instance.coins -= 100;
        DataManager.Instance.ball1 = true;
        DataManager.Instance.Save();
        coinText.text = (DataManager.Instance.coins + _coins).ToString();
    }
    public void BuyBall2()
    {
        DataManager.Instance.coins -= 200;
        DataManager.Instance.ball2 = true;
        DataManager.Instance.Save();
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
        DataManager.Instance.levelIndex++;
        DataManager.Instance.Save();
        for (int i = 0; i < confeties.Length; i++)
        {
            confeties[i].Play();
        }
    }
}
