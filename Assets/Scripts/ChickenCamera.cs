using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChickenCamera : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField, Range(0, 1)] float moveDuration = 0.2f;

    private void Start() 
    {
        //  offset camera memiliki posisi awal
        //  dan akan bergerak sesuai pergerakan karatakter
        offset = this.transform.position;    
    }

    public void UpdatePosition(Vector3 targetPosition)
    {
        DOTween.Kill(this.transform);
        transform.DOMove(offset + targetPosition, moveDuration);
    }
}

