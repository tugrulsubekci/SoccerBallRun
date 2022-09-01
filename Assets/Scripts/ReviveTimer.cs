using System.Collections;
using UnityEngine;
using TMPro;

public class ReviveTimer : MonoBehaviour
{
    private MenuManager menuManager;
    private TextMeshProUGUI timerText;
    private int _time = 3;


    void OnEnable()
    {
        menuManager = transform.parent.GetComponent<MenuManager>();
        timerText = GetComponent<TextMeshProUGUI>();
        StartCoroutine(ReviveTime());
    }

    IEnumerator ReviveTime()
    {
        while (_time > 0)
        {
            yield return new WaitForSeconds(1);
            _time--;
            timerText.text = _time.ToString();
        }
        timerText.gameObject.SetActive(false);
        menuManager.StartGame();
    }
}
