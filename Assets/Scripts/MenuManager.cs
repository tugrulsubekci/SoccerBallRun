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

    [SerializeField] TextMeshProUGUI smallerBallLevelText;
    [SerializeField] TextMeshProUGUI largerGoalLevelText;
    [SerializeField] TextMeshProUGUI smallerBallCostText;
    [SerializeField] TextMeshProUGUI largerGoalCostText;

    private int smallerBallLevel = 1;
    private int largerGoalLevel = 1;
    private int smallerBallCost;
    private int largerGoalCost;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        smallerBallCost = smallerBallLevel * 10;
        largerGoalCost = largerGoalLevel * 10;
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
        gameManager.isGameStarted = true;
        instructionalObjects.SetActive(true);
        menuObjects.SetActive(false);
    }

    public void SmallerBall()
    {
        if (gameManager.coins >= smallerBallCost)
        {
            ball.transform.localScale /= 1.05f;

            smallerBallLevel++;
            smallerBallLevelText.text = $"LEVEL {smallerBallLevel}";

            gameManager.AddCoin(-smallerBallCost);

            smallerBallCost = smallerBallLevel * 10;
            smallerBallCostText.text = smallerBallCost.ToString();
        }

    }

    public void LargerGoal()
    {
        if (gameManager.coins >= largerGoalCost)
        {
            goal.transform.localScale *= 1.05f;

            largerGoalLevel++;
            largerGoalLevelText.text = $"LEVEL {largerGoalLevel}";

            gameManager.AddCoin(-largerGoalCost);

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
}
