using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RandomMovement : MonoBehaviour
{

    public bool autoMove = false;

    private void Start(){
        if(autoMove){        
            transform.DOPunchPosition(Vector3.one, 3, 2).SetLoops(-1);
        }
    }
}
