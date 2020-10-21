using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;

    private float currentProgress = 0;

    // Start is called before the first frame update
    void Start()
    {
        //sets reference to slider
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        //animates the progress bar according to changes to the player progress
        if (slider.value < currentProgress)
        {
            slider.value += 0.5f * Time.deltaTime;

            slider.value = currentProgress;
        }
        else if (slider.value > currentProgress)
        {
            slider.value -= 0.5f * Time.deltaTime;

            slider.value = currentProgress;
        }
    }

    // function to change the progress value on the progress bar
    public void ChangeProgress(float value)
    {
        currentProgress = slider.value + value;
        
    }
}
