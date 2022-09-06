using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isGameStarted;
    public bool gameOver;
    public bool isLevelCompleted;
    public int _coins;

    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI shopCoinText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject levelCompletedPanel;
    [SerializeField] ParticleSystem[] confeties;

    private InterstitialAdvertisement interstitialAds;
    private AudioManager audioManager;
    private FBAnalyticss FBAnalyticss;
    private void Awake()
    {
#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = false;
#endif
        audioManager = FindObjectOfType<AudioManager>();
        FBAnalyticss = FindObjectOfType<FBAnalyticss>();
    }
    private void Start()
    {
        coinText.text = (DataManager.Instance.coins + _coins).ToString();
        levelText.text = $"LEVEL {DataManager.Instance.levelNumber + 1}";
        interstitialAds = GetComponent<InterstitialAdvertisement>();
        shopCoinText = levelCompletedPanel.transform.parent.GetChild(5).GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>();
    }
    public void AddCoin(int amount)
    {
        _coins += amount;
        RefreshCoinText();
    }
    public void PlayCoinSound()
    {
        audioManager.Play("Coin");
    }

    public void RefreshCoinText()
    {
        coinText.text = (DataManager.Instance.coins + _coins).ToString();
    }
    public void BuyItem(int price)
    {
        DataManager.Instance.coins -= price;
        coinText.text = DataManager.Instance.coins.ToString();
        shopCoinText.text = coinText.text;
    }
    public bool HasEnoughCoins(int price)
    {
        return DataManager.Instance.coins >= price;
    }
    public void GameOver()
    {
        audioManager.Play("GameOver");
        gameOver = true;
        GameObject tapToShootText = levelCompletedPanel.transform.parent.GetChild(6).gameObject;
        tapToShootText.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void LevelCompleted()
    {
        audioManager.Play("LevelCompleted");
        audioManager.Play("Confetti");
        isLevelCompleted = true;

        TextMeshProUGUI gainedCoinsText = levelCompletedPanel.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        gainedCoinsText.text = _coins.ToString();

        GameObject tapToShootText = levelCompletedPanel.transform.parent.GetChild(6).gameObject;
        tapToShootText.SetActive(false);

        levelCompletedPanel.SetActive(true);

        for (int i = 0; i < confeties.Length; i++)
        {
            confeties[i].Play();
        }

        if(!DataManager.Instance.noAds)
        {
            interstitialAds.ShowAd();
        }
        DataManager.Instance.coins += _coins;
        DataManager.Instance.levelIndex++;
        DataManager.Instance.levelNumber++;
        DataManager.Instance.Save();
        FBAnalyticss.LogAchievedLevelEvent("Level " + DataManager.Instance.levelNumber.ToString());
    }
    public void Revive()
    {
        gameOver = false;
        isGameStarted = false;
        gameOverPanel.SetActive(false);
        gameOverPanel.transform.parent.GetChild(0).gameObject.SetActive(true);
    }
}
