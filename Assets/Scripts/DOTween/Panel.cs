using DG.Tweening;
using UnityEngine;

public class Panel : MonoBehaviour
{
    private Transform panelTrans;
    void Start()
    {
        panelTrans = transform;
    }

    public void ScaleX()
    {
        panelTrans.DOScaleX(0, 0.6f);
        Destroy(gameObject, 0.6f);
    }
    private void OnDestroy()
    {
        panelTrans.DOKill();
    }
}
