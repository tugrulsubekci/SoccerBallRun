using DG.Tweening;
using UnityEngine;

public class ScaleText : MonoBehaviour
{
    private Transform textTrans;
    [SerializeField] float duration = 1.0f;
    [SerializeField] float scale1 = 0.8f;
    [SerializeField] float scale2 = 1.0f;
    [SerializeField] Ease ease;
    private LoopType loopType = LoopType.Yoyo;
    void Start()
    {
        textTrans = transform;
        textTrans
            .DOScale(scale1, duration)
            .OnComplete(() =>
            {
                textTrans.DOScale(scale2, duration);
            })
            .SetLoops(-1, loopType)
            .SetEase(ease);
    }
    private void OnEnable()
    {
        textTrans.DOPlay();
    }
    private void OnDisable()
    {
        textTrans.DOPause();
    }
}
