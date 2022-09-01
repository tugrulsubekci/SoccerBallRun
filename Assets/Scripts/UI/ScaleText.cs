using UnityEngine;
using DG.Tweening;
using System.Runtime.CompilerServices;

public class ScaleText : MonoBehaviour
{
    private Transform textTrans;
    void Start()
    {
        textTrans = transform;
        textTrans.DOScale(1.5f, 2);

    }

}
