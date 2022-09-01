using DG.Tweening;
using UnityEngine;

public class Rolling : MonoBehaviour
{
    private Transform playerTrans;
    private Vector3 rotation = new Vector3(360, 0, 0);
    private float duration = 1.2f;
    private void OnEnable()
    {
        playerTrans = transform;
    }

    public void Roll()
    {
        playerTrans.rotation = Quaternion.Euler(Vector3.zero);
        playerTrans.DORotate(rotation, duration, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
    }
    public void Stop()
    {
        playerTrans.DOKill();
    }
    private void OnDisable()
    {
        playerTrans.DOKill();
    }
}
