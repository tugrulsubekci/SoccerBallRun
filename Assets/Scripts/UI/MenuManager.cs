using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject instructionalObjects;
    [SerializeField] GameObject menuObjects;
    [SerializeField] GameObject ball;
    [SerializeField] GameObject goal;
    [SerializeField] GameObject settingsObjects;
    [SerializeField] GameObject mute;
    [SerializeField] GameObject unmute;
    [SerializeField] GameObject shopPanel;

    [SerializeField] TextMeshProUGUI smallerBallLevelText;
    [SerializeField] TextMeshProUGUI largerGoalLevelText;

    [SerializeField] TextMeshProUGUI smallerBallCostText;
    [SerializeField] TextMeshProUGUI largerGoalCostText;

    private int smallerBallLevel = 1;
    private int largerGoalLevel = 1;
    private int smallerBallCost;
    private int largerGoalCost;

    private GameManager gameManager;
    private void Awake()
    {
        shopPanel.SetActive(true);
    }
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        smallerBallLevel += DataManager.Instance.smallerSkill_Level;
        smallerBallCost = smallerBallLevel * 10;
        smallerBallLevelText.text = $"LEVEL {smallerBallLevel}";
        smallerBallCostText.text = smallerBallCost.ToString();
        for (int i = 0; i < smallerBallLevel; i++)
        {
            ball.transform.localScale /= 1.05f;
        }

        largerGoalLevel += DataManager.Instance.largerSkill_Level;
        largerGoalCost = largerGoalLevel * 10;
        largerGoalCostText.text = largerGoalCost.ToString();
        largerGoalLevelText.text = $"LEVEL {largerGoalLevel}";
        for (int i = 0; i < largerGoalLevel; i++)
        {
            goal.transform.localScale *= 1.05f;
        }

        if(DataManager.Instance.isMusicOn)
        {
            mute.SetActive(true);
            unmute.SetActive(false);
        }
        else
        {
            mute.SetActive(false);
            unmute.SetActive(true);
        }

    }

    private void Update()
    {
        if (gameManager.isGameStarted && Input.GetMouseButtonDown(0))
        {
            instructionalObjects.SetActive(false);
        }
    }

    public void StartGame()
    {
        FindObjectOfType<AudioManager>().Play("Start");        
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            instructionalObjects.SetActive(true);
        }
        menuObjects.SetActive(false);
        Invoke(nameof(StartNow), 0.5f);
    }
    public void StartNow()
    {
        gameManager.isGameStarted = true;
    }
    public void SmallerBall()
    {
        if (DataManager.Instance.coins + gameManager._coins >= smallerBallCost)
        {
            ball.transform.localScale /= 1.05f;

            smallerBallLevel++;
            smallerBallLevelText.text = $"LEVEL {smallerBallLevel}";

            gameManager.AddCoin(-smallerBallCost);
            LevelUp(nameof(SmallerBall));

            smallerBallCost = smallerBallLevel * 10;
            smallerBallCostText.text = smallerBallCost.ToString();
        }
    }

    public void LargerGoal()
    {
        if (DataManager.Instance.coins + gameManager._coins >= largerGoalCost)
        {
            goal.transform.localScale *= 1.05f;

            largerGoalLevel++;
            largerGoalLevelText.text = $"LEVEL {largerGoalLevel}";

            gameManager.AddCoin(-largerGoalCost);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void SettingsButton()
    {
        FindObjectOfType<AudioManager>().Play("Click");
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
        FindObjectOfType<AudioManager>().Play("Click");
        if (mute.activeInHierarchy)
        {
            DataManager.Instance.isMusicOn = false;
            mute.SetActive(false);
            unmute.SetActive(true);
        }
        else
        {
            DataManager.Instance.isMusicOn = true;
            mute.SetActive(true);
            unmute.SetActive(false);
        }
        DataManager.Instance.Save();
    }
    public void ShopButton()
    {
        FindObjectOfType<AudioManager>().Play("Click");
        if(!shopPanel.activeInHierarchy)
        {
            shopPanel.SetActive(true);
        }
        else
        {
            shopPanel.SetActive(false);
        }
    }
    private void LevelUp(string skillName)
    {
        FindObjectOfType<AudioManager>().Play("LevelUp");
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
}