using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] TextMeshProUGUI blackBallButtonText;
    [SerializeField] TextMeshProUGUI blueBallButtonText;
    [SerializeField] TextMeshProUGUI yellowBallButtonText;

    [SerializeField] GameObject blackBallEffect;
    [SerializeField] GameObject blueBallEffect;
    [SerializeField] GameObject yellowBallEffect;

    [SerializeField] GameObject blackBall;
    [SerializeField] GameObject blueBall;
    [SerializeField] GameObject yellowBall;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void Cancel()
    {
        gameObject.SetActive(false);
    }
    public void BlackBallButton()
    {
        if (blackBallButtonText.text == "Select")
        {
            gameManager.ballIndex = 0;
            blackBall.SetActive(true);
            blackBallEffect.SetActive(true);
            blackBallButtonText.text = "Selected";
            if (yellowBallButtonText.text == "Selected")
            {
                yellowBall.SetActive(false);
                yellowBallEffect.SetActive(false);
                yellowBallButtonText.text = "Select";
            }
            else if (blueBallButtonText.text == "Selected")
            {
                blueBall.SetActive(false);
                blueBallEffect.SetActive(false);
                blueBallButtonText.text = "Select";
            }
        }
    }
    public void BlueBallButton()
    {
        if (blueBallButtonText.text == "Buy")
        {
            if (gameManager.coins >= 100)
            {
                gameManager.AddCoin(-100);
                blueBallButtonText.text = "Select";
            }
        }
        else if (blueBallButtonText.text == "Select")
        {
            gameManager.ballIndex = 1;
            blueBall.SetActive(true);
            blueBallEffect.SetActive(true);
            blueBallButtonText.text = "Selected";
            if (blackBallButtonText.text == "Selected")
            {
                blackBall.SetActive(false);
                blackBallEffect.SetActive(false);
                blackBallButtonText.text = "Select";
            }
            else if (yellowBallButtonText.text == "Selected")
            {
                yellowBall.SetActive(false);
                yellowBallEffect.SetActive(false);
                yellowBallButtonText.text = "Select";
            }
        }
    }
    public void YellowBallButton()
    {
        if (yellowBallButtonText.text == "Buy")
        {
            if (gameManager.coins >= 200)
            {
                gameManager.AddCoin(-200);
                yellowBallButtonText.text = "Select";
            }
        }
        else if (yellowBallButtonText.text == "Select")
        {
            gameManager.ballIndex = 2;
            yellowBall.SetActive(true);
            yellowBallEffect.SetActive(true);
            yellowBallButtonText.text = "Selected";
            if (blackBallButtonText.text == "Selected")
            {
                blackBall.SetActive(false);
                blackBallEffect.SetActive(false);
                blackBallButtonText.text = "Select";
            }
            else if (blueBallButtonText.text == "Selected")
            {
                blueBall.SetActive(false);
                blueBallEffect.SetActive(false);
                blueBallButtonText.text = "Select";
            }
        }
    }
}
