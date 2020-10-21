using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeBehaviour : MonoBehaviour
{
    //camera transform
    private Transform cam;

    //duration of shake
    private float shakeDuration;

    //shake magnitude
    private float shakeMagnitude = 1f;

    //shake damping speed
    private float shakeDampening = 1f;

    //initial location of camera
    Vector3 initialPos;

    //sets reference to camera transform
    void Awake()
    {
        if (cam == null)
        {
            cam = GetComponent<Transform>(); 
        }
    }

    //sets original position of camera
    void OnEnable()
    {
        initialPos = cam.localPosition;
    }

    // camera shake effect
    void Update()
    {
        if (shakeDuration > 0)
        {
            cam.localPosition = initialPos + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * shakeDampening;
        }
        else
        {
            shakeDuration = 0;
            cam.localPosition = initialPos;
        }
    }

    //function to set a duration to shake the screen for
    public void TriggerShake(float duration)
    {
        shakeDuration = duration;
    }
}
