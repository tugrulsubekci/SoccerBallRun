using DG.Tweening;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private Vector3 _eulerAngles = new Vector3(0, 360, 0);
    private Quaternion zeroQ;
    private Transform goTrans;
    private void OnEnable()
    {
        goTrans = transform;
        zeroQ = Quaternion.Euler(Vector3.zero);
        goTrans.rotation = zeroQ;
        goTrans.DORotate(_eulerAngles, 10, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
    }
    private void OnDisable()
    {
        goTrans.DOKill();
    }
}