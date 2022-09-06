using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] public GameObject menuObjects;
    [SerializeField] GameObject ball;
    [SerializeField] GameObject goal;
    [SerializeField] GameObject settingsObjects;
    [SerializeField] GameObject mute;
    [SerializeField] GameObject vibration;
    [SerializeField] GameObject shopPanel;

    [SerializeField] TextMeshProUGUI smallerBallLevelText;
    [SerializeField] TextMeshProUGUI largerGoalLevelText;

    [SerializeField] TextMeshProUGUI smallerBallCostText;
    [SerializeField] TextMeshProUGUI largerGoalCostText;

    private int smallerBallLevel = 1;
    private int largerGoalLevel = 1;
    private int smallerBallCost;
    private int largerGoalCost;
    private float scaleMultiplier = 1.025f;

    public GameManager gameManager;
    public AudioManager audioManager;
    private Camera mainCamera;
    public GameObject player;
    private void Awake()
    {
        shopPanel.SetActive(true);
    }
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();
        mainCamera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");

        UpdateSmaller();

        UpdateLarger();

        if (DataManager.Instance.isMusicOn)
        {
            mute.SetActive(false);
        }
        else
        {
            mute.SetActive(true);
        }

    }
    public void SmallerBall()
    {
        if (gameManager.HasEnoughCoins(smallerBallCost))
        {
            ball.transform.localScale /= scaleMultiplier;

            smallerBallLevel++;
            smallerBallLevelText.text = $"LEVEL {smallerBallLevel}";

            gameManager.BuyItem(smallerBallCost);
            LevelUp(nameof(SmallerBall));


            smallerBallCost = smallerBallLevel * 10;
            smallerBallCostText.text = smallerBallCost.ToString();
        }
    }

    public void LargerGoal()
    {
        if (gameManager.HasEnoughCoins(largerGoalCost))
        {
            goal.transform.localScale *= scaleMultiplier;

            largerGoalLevel++;
            largerGoalLevelText.text = $"LEVEL {largerGoalLevel}";

            gameManager.BuyItem(largerGoalCost);
            LevelUp(nameof(LargerGoal));

            largerGoalCost = largerGoalLevel * 10;
            largerGoalCostText.text = largerGoalCost.ToString();
        }
    }
    public void NoThanks()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void NextLevel()
    {
        if (DataManager.Instance.levelIndex < 10)
        {
            SceneManager.LoadScene(DataManager.Instance.levelIndex);
        }
        else if (DataManager.Instance.levelIndex == 10)
        {
            DataManager.Instance.levelIndex = 1;
            DataManager.Instance.Save();
            SceneManager.LoadScene(DataManager.Instance.levelIndex);
        }
    }
    public void SettingsButton()
    {
        audioManager.Play("Click");
        if (settingsObjects.activeInHierarchy)
        {
            settingsObjects.SetActive(false);
        }
        else
        {
            settingsObjects.SetActive(true);
        }
    }
    public void Mute()
    {
        audioManager.Play("Click");
        if (mute.activeInHierarchy)
        {
            DataManager.Instance.isMusicOn = true;
            mute.SetActive(false);
        }
        else
        {
            DataManager.Instance.isMusicOn = false;
            mute.SetActive(true);
        }
        DataManager.Instance.Save();
    }
    public void Vibration()
    {
        audioManager.Play("Click");
        if (vibration.activeInHierarchy)
        {
            DataManager.Instance.isVibrationOn = true;
            vibration.SetActive(false);
        }
        else
        {
            DataManager.Instance.isVibrationOn = false;
            vibration.SetActive(true);
        }
        DataManager.Instance.Save();
    }
    public void ShopButton()
    {
        audioManager.Play("Click");
        if (!shopPanel.activeInHierarchy)
        {
            shopPanel.SetActive(true);
            menuObjects.SetActive(false);
            mainCamera.cullingMask = 0;
        }
        else
        {
            shopPanel.SetActive(false);
            menuObjects.SetActive(true);
            mainCamera.cullingMask = -1;
        }
    }
    private void LevelUp(string skillName)
    {
        audioManager.Play("LevelUp");
        if (skillName == "LargerGoal")
        {
            DataManager.Instance.coins += gameManager._coins;
            DataManager.Instance.largerSkill_Level++;
            DataManager.Instance.Save();
        }
        else if (skillName == "SmallerBall")
        {
            DataManager.Instance.coins += gameManager._coins;
            DataManager.Instance.smallerSkill_Level++;
            DataManager.Instance.Save();
        }
    }

    private void UpdateSmaller()
    {
        smallerBallLevel += DataManager.Instance.smallerSkill_Level;
        smallerBallCost = smallerBallLevel * 10;
        smallerBallLevelText.text = $"LEVEL {smallerBallLevel}";
        smallerBallCostText.text = smallerBallCost.ToString();
        for (int i = 0; i < smallerBallLevel; i++)
        {
            ball.transform.localScale /= scaleMultiplier;
        }
    }

    private void UpdateLarger()
    {
        largerGoalLevel += DataManager.Instance.largerSkill_Level;
        largerGoalCost = largerGoalLevel * 10;
        largerGoalCostText.text = largerGoalCost.ToString();
        largerGoalLevelText.text = $"LEVEL {largerGoalLevel}";
        for (int i = 0; i < largerGoalLevel; i++)
        {
            goal.transform.localScale *= scaleMultiplier;
        }
    }
}