using System;
using DG.Tweening;
using UnityEngine;

public class TreeSlot : MonoBehaviour
{
    public bool isFree = true;
    [SerializeField] private Vector3 loopRotation = new Vector3(0, 20, 20);
    [SerializeField] private float animTime = 1;

    private void Start()
    {
        Vector3 startRotation = transform.localRotation.eulerAngles;
        transform.DOLocalRotate(startRotation - loopRotation / 2, animTime)
            .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    public void HangDecoration()
    {
        isFree = false;
        GetComponent<MeshRenderer>().enabled = false;
    }
}