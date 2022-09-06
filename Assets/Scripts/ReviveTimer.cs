using System.Collections;
using TMPro;
using UnityEngine;

public class ReviveTimer : MonoBehaviour
{
    private HoldAndMove holdAndMove;
    private TextMeshProUGUI timerText;
    private int _time = 3;


    void OnEnable()
    {
        holdAndMove = transform.parent.GetComponent<HoldAndMove>();
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
        holdAndMove.StartGame();
    }
}
