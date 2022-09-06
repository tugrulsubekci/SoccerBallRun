using DG.Tweening;
using UnityEngine;

public class FlipCoin : MonoBehaviour
{
    private Transform coinTrans;
    private Vector3 rotation = new Vector3(0, 360, 0);
    private float duration = 2;
    void Start()
    {
        coinTrans = transform;
        coinTrans.DORotate(rotation, duration, RotateMode.FastBeyond360).SetLoops(-1);
    }
    private void OnDestroy()
    {
        coinTrans.DOKill();
    }
}

