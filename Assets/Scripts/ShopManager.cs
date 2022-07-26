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

    private bool Started = false;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    private void Start()
    {
        switch (DataManager.Instance.ballIndex)
        {
            case 0:
                blackBallButtonText.text = "Selected";
                blackBall.SetActive(true);
                blackBallEffect.SetActive(true);

                adidasBall.SetActive(false);
                adidasBallEffect.SetActive(false);
                if (DataManager.Instance.ball1)
                {
                    adidasBallButtonText.text = "Select";
                }
                else
                {
                    adidasBallButtonText.text = "Buy";
                }

                cafusaBall.SetActive(false);
                cafusaBallEffect.SetActive(false);
                if (DataManager.Instance.ball2)
                {
                    cafusaBallButtonText.text = "Select";
                }
                else
                {
                    cafusaBallButtonText.text = "Buy";
                }

                break;

            case 1:
                adidasBallButtonText.text = "Selected";
                adidasBall.SetActive(true);
                adidasBallEffect.SetActive(true);

                cafusaBall.SetActive(false);
                cafusaBallEffect.SetActive(false);
                if (DataManager.Instance.ball2)
                {
                    cafusaBallButtonText.text = "Select";
                }
                else
                {
                    cafusaBallButtonText.text = "Buy";
                }

                blackBall.SetActive(false);
                blackBallEffect.SetActive(false);
                blackBallButtonText.text = "Select";

                break;

            case 2:
                cafusaBallButtonText.text = "Selected";
                cafusaBall.SetActive(true);
                cafusaBallEffect.SetActive(true);

                blackBall.SetActive(false);
                blackBallEffect.SetActive(false);
                blackBallButtonText.text = "Select";

                adidasBall.SetActive(false);
                adidasBallEffect.SetActive(false);
                if (DataManager.Instance.ball1)
                {
                    adidasBallButtonText.text = "Select";
                }
                else
                {
                    adidasBallButtonText.text = "Buy";
                }

                break;
        }
        if (!Started)
        {
            this.gameObject.SetActive(false);
            Started = true;
        }

    }
    public void Cancel()
    {
        FindObjectOfType<AudioManager>().Play("Click");
        gameObject.SetActive(false);
    }
    public void BlackBallButton()
    {
        FindObjectOfType<AudioManager>().Play("Click");
        if (blackBallButtonText.text == "Select")
        {
            blackBall.SetActive(true);
            blackBallEffect.SetActive(true);
            SaveBallIndex(0);
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
        FindObjectOfType<AudioManager>().Play("Click");
        if (adidasBallButtonText.text == "Buy")
        {
            if (DataManager.Instance.coins + gameManager._coins >= 100)
            {
                gameManager.BuyBall1();
                adidasBallButtonText.text = "Select";
            }
        }
        else if (adidasBallButtonText.text == "Select")
        {
            adidasBall.SetActive(true);
            adidasBallEffect.SetActive(true);
            adidasBallButtonText.text = "Selected";
            SaveBallIndex(1);
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
        FindObjectOfType<AudioManager>().Play("Click");
        if (cafusaBallButtonText.text == "Buy")
        {
            if (DataManager.Instance.coins + gameManager._coins >= 200)
            {
                gameManager.BuyBall2();
                cafusaBallButtonText.text = "Select";
            }
        }
        else if (cafusaBallButtonText.text == "Select")
        {
            cafusaBall.SetActive(true);
            cafusaBallEffect.SetActive(true);
            cafusaBallButtonText.text = "Selected";
            SaveBallIndex(2);
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
    private void SaveBallIndex(int ballInd)
    {
        DataManager.Instance.ballIndex = ballInd;
        DataManager.Instance.Save();
    }
}
