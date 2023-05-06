using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{

    [SerializeField] int value = 1;
    [SerializeField, Range(0, 100)] float rotationSpeed;

    public int Value { get => value; }


    private void selfDesctruct()
    {
        Destroy(this.gameObject);
    }

    public void Collected()
    {
        //  Code untuk mematikan collider pada coin
        //  ketika terkena ayam, agar tidak terjadi
        //  penambahan 2x dalam pengambilan coin tersebut
        GetComponent<Collider>().enabled = false;

        rotationSpeed *= 200;

        this.transform.DOJump(this.transform.position, 1.5f, 1, 0.6f).onComplete = selfDesctruct;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 360 * rotationSpeed * Time.deltaTime, 0 );
    }
}
