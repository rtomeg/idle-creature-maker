using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Eyeball : MonoBehaviour
{
    [SerializeField]
    private Transform pupil;
    void Start()
    {
        StartCoroutine(RandomMovement());
    }

private IEnumerator RandomMovement(){
    
    transform.DOLocalRotate(new Vector3(Random.Range(-33, 33), Random.Range(-33, 33), Random.Range(-33,33)), Random.Range(0.1f, 0.2f));
    float waitTime = Random.Range(0.1f, 1);
    yield return new WaitForSeconds(waitTime);
    pupil.DOScale(Random.Range(0.9f, 1.2f), waitTime);
    yield return new WaitForSeconds(waitTime);
    pupil.DOScale(Random.Range(0.9f, 1.2f), waitTime);
    StartCoroutine(RandomMovement());
}
}
