using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] TextMeshProUGUI blackBallButtonText;
    [SerializeField] TextMeshProUGUI adidasBallButtonText;
    [SerializeField] TextMeshProUGUI cafusaBallButtonText;

    [SerializeField] GameObject blackBallEffect;
    [SerializeField] GameObject adidasBallEffect;
    [SerializeField] GameObject cafusaBallEffect;

    [SerializeField] GameObject blackBall;
    [SerializeField] GameObject adidasBall;
    [SerializeField] GameObject cafusaBall;

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
            if (cafusaBallButtonText.text == "Selected")
            {
                cafusaBall.SetActive(false);
                cafusaBallEffect.SetActive(false);
                cafusaBallButtonText.text = "Select";
            }
            else if (adidasBallButtonText.text == "Selected")
            {
                adidasBall.SetActive(false);
                adidasBallEffect.SetActive(false);
                adidasBallButtonText.text = "Select";
            }
        }
    }
    public void BlueBallButton()
    {
        if (adidasBallButtonText.text == "Buy")
        {
            if (DataManager.Instance.coins + gameManager._coins >= 100)
            {
                gameManager.AddCoin(-100);
                adidasBallButtonText.text = "Select";
            }
        }
        else if (adidasBallButtonText.text == "Select")
        {
            gameManager.ballIndex = 1;
            adidasBall.SetActive(true);
            adidasBallEffect.SetActive(true);
            adidasBallButtonText.text = "Selected";
            if (blackBallButtonText.text == "Selected")
            {
                blackBall.SetActive(false);
                blackBallEffect.SetActive(false);
                blackBallButtonText.text = "Select";
            }
            else if (cafusaBallButtonText.text == "Selected")
            {
                cafusaBall.SetActive(false);
                cafusaBallEffect.SetActive(false);
                cafusaBallButtonText.text = "Select";
            }
        }
    }
    public void YellowBallButton()
    {
        if (cafusaBallButtonText.text == "Buy")
        {
            if (DataManager.Instance.coins + gameManager._coins >= 200)
            {
                gameManager.AddCoin(-200);
                cafusaBallButtonText.text = "Select";
            }
        }
        else if (cafusaBallButtonText.text == "Select")
        {
            gameManager.ballIndex = 2;
            cafusaBall.SetActive(true);
            cafusaBallEffect.SetActive(true);
            cafusaBallButtonText.text = "Selected";
            if (blackBallButtonText.text == "Selected")
            {
                blackBall.SetActive(false);
                blackBallEffect.SetActive(false);
                blackBallButtonText.text = "Select";
            }
            else if (adidasBallButtonText.text == "Selected")
            {
                adidasBall.SetActive(false);
                adidasBallEffect.SetActive(false);
                adidasBallButtonText.text = "Select";
            }
        }
    }
}
