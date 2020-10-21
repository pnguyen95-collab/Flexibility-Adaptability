using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenColour : MonoBehaviour
{
    //reference to bg image
    private Image bgImage;

    //duration of colour change
    private float flashDuration;

    //boolean to check if success or fail
    private bool win;

    //original colour of bg image
    private Color originalColor;

    //sets variables on awake
    void Awake()
    {
        bgImage = GetComponent<Image>();

        originalColor = bgImage.color;
    }

    //screen colour flash
    void Update()
    {
        if (flashDuration > 0)
        {
            if (win)
            {
                bgImage.color = Color.green;
            }
            else
            {
                bgImage.color = Color.red;
            }

            flashDuration -= Time.deltaTime * 1f;
        }
        else
        {
            flashDuration = 0;
            bgImage.color = originalColor;
            win = false;
        }
    }

    //functions to trigger screen flash when win/lose
    public void WinTrigger()
    {
        win = true;
        flashDuration = 0.4f;
    }

    public void LoseTrigger()
    {
        win = false;
        flashDuration = 0.4f;
    }
}
