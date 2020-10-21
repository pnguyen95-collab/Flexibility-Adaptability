using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    //function called to change scenes
    public void ToScene(string newScene)
    {
        SceneManager.LoadScene(newScene);
    }

    //function called to exit the game
    public void Exit()
    {
        Application.Quit();
    }
}
