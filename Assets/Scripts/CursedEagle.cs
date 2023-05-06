using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedEagle : MonoBehaviour
{
    [SerializeField, Range(0, 50)] float moveSpeedEagle = 25;

    private void Update()
    {
        transform.Translate(Vector3.forward * moveSpeedEagle * Time.deltaTime);    
    }
}
