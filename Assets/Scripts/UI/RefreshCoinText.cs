using TMPro;
using UnityEngine;

public class RefreshCoinText : MonoBehaviour
{
    private TextMeshProUGUI coinText;
    private TextMeshProUGUI gamePlayCoinText;

    private void OnEnable()
    {
        RefreshText();
    }

    public void RefreshText()
    {
        coinText = transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>();
        gamePlayCoinText = transform.parent.GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        coinText.text = gamePlayCoinText.text;
    }
}
