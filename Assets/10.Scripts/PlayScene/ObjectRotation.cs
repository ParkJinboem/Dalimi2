using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ObjectRotation : MonoBehaviour
{
    [SerializeField] private bool forward;

    public void OnEnable()
    {
        if(forward)
        {
            transform.DOLocalRotate(new Vector3(0, 0, -360), 10f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
        }
        else
        {
            transform.DOLocalRotate(new Vector3(0, 0, 360), 10f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
        }
    }

    public void OnDisable()
    {
        transform.DOPause();
    }

}
