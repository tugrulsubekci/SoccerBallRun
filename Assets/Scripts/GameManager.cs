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
