using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverText : MonoBehaviour
{
    [SerializeField] TMP_Text gameOverText;
    [SerializeField] TMP_Text highScore;


    private void Start() 
    {
        highScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();    
    }


    public void UpdateText(int score)
    {
        int number = score;

        gameOverText.text = "GAME OVER!!!\nScore: " + score;

        if (number > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", number);
        }
    }
}
