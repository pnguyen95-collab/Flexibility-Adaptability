using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject howToPlayScreen;
    private bool howToPlay = false;

    //function called to change scenes
    public void ToScene(string newScene)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(newScene);
    }

    //function called to exit the game
    public void Exit()
    {
        Application.Quit();
    }

    //function to handle the how to play screen
    public void HowToPlay()
    {
        if (howToPlay)
        {
            howToPlayScreen.SetActive(false);
            howToPlay = false;
        }
        else
        {
            howToPlayScreen.SetActive(true);
            howToPlay = true;
        }
    }
}
