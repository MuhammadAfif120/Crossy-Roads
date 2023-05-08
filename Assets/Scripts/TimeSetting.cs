using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeSetting : MonoBehaviour
{
    public TMP_Text timerText;
    public TMP_Text finatlTimer;
    public float timerAmount;
    public float timerIncreaseperSec;

    // Start is called before the first frame update
    void Start()
    {
        timerAmount = 0f;
        timerIncreaseperSec = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = (int)timerAmount + " Sec";
        timerAmount += timerIncreaseperSec * Time.deltaTime;
    }
}
