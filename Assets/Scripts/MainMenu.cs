using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private void Start() 
    {
        Screen.SetResolution(1080, 1920, true);    
    }

    public void Play()
    {
        SceneManager.LoadScene("Main Crossy Road");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
