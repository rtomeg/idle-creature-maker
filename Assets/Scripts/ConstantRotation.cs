using System;
using DG.Tweening;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    private Tween rot;
    [SerializeField] private float duration = 2f;
    [SerializeField] private bool clockwise = true;

    private void Start()
    {
        Vector3 targetRotation = clockwise ? Vector3.up : Vector3.down;
        rot = transform.DOLocalRotate(targetRotation*360, duration, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
    }

    private void OnDestroy()
    {
        rot.Kill();
    }
}
