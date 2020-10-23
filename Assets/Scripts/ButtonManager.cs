using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    //reference to game manager
    public GameObject gm;
    private GameManager gmCode;

    //reference to number of colours to remember
    private int colourCount;

    private void Start()
    {
        gmCode = gm.GetComponent<GameManager>();
    }

    //function that handles what happens when you click on the button
    public void ButtonPress(GameObject button)
    {
        //checks if it's currently memory minigame otherwise do nothing
        if (gmCode.memoryActive)
        {
            //adds the specified button to the current selection
            gmCode.currentSelection.Add(button);

            colourCount = gmCode.memoryOrder.Count;

            //compares the current index of buttons in the order and if correct keep checking until the player has gotten them all or failed on one

        }
    }
}
