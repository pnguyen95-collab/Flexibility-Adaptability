using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //references to game progress and visual effects gameobjects
    public GameObject cam;
    public GameObject bgImage;
    public GameObject progressBar;

    private ShakeBehaviour shake;
    private ScreenColour tintBg;
    private ProgressBar progress;

    [Header("Input Mini-Game")]
    //input minigame canvas
    public GameObject inputScreen;

    //input minigame variables
    private bool inputActive;
    private string[] inputOptions = new string[26]{"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};
    private int randomIndex;
    private string inputCharacter;
    public Text inputText;

    [Header("Memory Mini-Game")]
    //memory minigame canvas/panels
    public GameObject memoryScreen;
    public GameObject memorizePanel;
    public GameObject directionSpawn;
    public GameObject replicatePanel;

    //button prefabs to spawn
    public GameObject redButtonPrefab;
    public GameObject blueButtonPrefab;
    public GameObject yellowButtonPrefab;
    public GameObject pinkButtonPrefab;
    public GameObject tealButtonPrefab;
    public GameObject greenButtonPrefab;
    public GameObject rightDirection;
    public GameObject leftDirection;

    //memory minigame variables
    private bool memoryActive;
    private List<GameObject> memoryOrder = new List<GameObject>();
    private List<GameObject> currentSelection = new List<GameObject>();
    private float memorizeTime;
    private float replicateTime;
    private bool replicate;

    //difficulty and variation progression
    private int completionAmount = 0;
    private int difficultyTier = 0;
    private bool directionSwap;

    //timers
    private float memoryGameCountdown;

    // Start is called before the first frame update
    void Start()
    {
        shake = cam.GetComponent<ShakeBehaviour>();
        tintBg = bgImage.GetComponent<ScreenColour>();
        progress = progressBar.GetComponent<ProgressBar>();

        StartInput();
        //sets up the countdown before the memory minigame
        memoryGameCountdown = UnityEngine.Random.Range(5f, 8f);
    }

    // Update is called once per frame
    void Update()
    {
        //if input minigame is active will take into account key pressed
        if (inputActive)
        {
            //checks to see if you have pressed a key
            if (Input.anyKeyDown && !(Input.GetMouseButtonDown(0)|| Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
            {
                KeyCode thisKey = (KeyCode)Enum.Parse(typeof(KeyCode), inputCharacter);

                //compares the key pressed and the key that's meant to be pressed
                if (Input.GetKeyDown(thisKey) == true)
                {
                    //visual effects & score update
                    tintBg.WinTrigger();
                    progress.ChangeProgress(0.05f);

                    //win condition check
                    if(progress.slider.value == 1f)
                    {
                        //win screen here
                    }

                    //resets input character
                    randomIndex = UnityEngine.Random.Range(0, 26);

                    inputCharacter = inputOptions[randomIndex];

                    inputText.text = "Press " + inputCharacter + "!";
                }
                else
                {
                    //visual effects & score update
                    tintBg.LoseTrigger();
                    shake.TriggerShake(0.4f);

                    //resets input character
                    randomIndex = UnityEngine.Random.Range(0, 26);

                    inputCharacter = inputOptions[randomIndex];

                    inputText.text = "Press " + inputCharacter + "!";
                }
            }

            //if the memory game countdown hits 0, engage in the memory minigame
            if (memoryGameCountdown > 0)
            {
                memoryGameCountdown -= Time.deltaTime * 1f;
            }
            else
            {
                //changes bg color to a different color
                bgImage.GetComponent<Image>().color = Color.yellow;

                //turns off input minigame and activates the memory minigame
                inputActive = false;
                inputScreen.SetActive(false);

                StartMemory();
            }
        }
    }

    //function to start the input game
    public void StartInput()
    {
        inputActive = true;

        if (inputScreen != isActiveAndEnabled)
        {
            inputScreen.SetActive(true);
        }

        //randomises input option & updates input text accordingly
        randomIndex = UnityEngine.Random.Range(0, 26);

        inputCharacter = inputOptions[randomIndex];

        inputText.text = "Press " + inputCharacter + "!";
    }

    //function to start the memory game
    public void StartMemory()
    {
        memoryActive = true;

        //sets up the memorize panel with a switch based on current difficulty
        switch(difficultyTier)
        {
            case 0: //lowest difficulty only right direction and 3 colours to remember

                memoryOrder = new List<GameObject>();
                directionSwap = false;

                for(int i = 0; i < 3; i++)
                {
                    int randomColor = UnityEngine.Random.Range(0, 6);

                    CreateColorArray(randomColor);
                }

                break;

            case 1: //medium difficulty only left direction and 4 colours to remember

                memoryOrder = new List<GameObject>();
                directionSwap = true;

                for(int i = 0; i > 4; i++)
                {
                    int randomColor = UnityEngine.Random.Range(0, 6);

                    CreateColorArray(randomColor);
                }

                break;

            case 2: //highest difficulty can be either direction and 5 colours to remember

                memoryOrder = new List<GameObject>();
                int random = UnityEngine.Random.Range(0, 2);

                if (random == 0)
                {
                    directionSwap = false;
                }
                else
                {
                    directionSwap = true;
                }

                for (int i = 0; i > 5; i++)
                {
                    int randomColor = UnityEngine.Random.Range(0, 6);

                    CreateColorArray(randomColor);
                }

                break;
        }

        //instantiates the colour buttons
        foreach(GameObject colour in memoryOrder)
        {

        }

        //displays the memorize panel and sets the timers
        memorizePanel.SetActive(true);
        memorizeTime = 2f;
    }

    //function to populate the list
    public void CreateColorArray(int colorIndex)
    {
        //adds red
        if (colorIndex == 0)
        {
            memoryOrder.Add(redButtonPrefab);
        }

        //adds blue
        if (colorIndex == 1)
        {
            memoryOrder.Add(blueButtonPrefab);
        }

        //adds yellow
        if (colorIndex == 2)
        {
            memoryOrder.Add(yellowButtonPrefab);
        }

        //adds pink
        if (colorIndex == 3)
        {
            memoryOrder.Add(pinkButtonPrefab);
        }

        //adds teal
        if (colorIndex == 4)
        {
            memoryOrder.Add(tealButtonPrefab);
        }

        //adds green
        if (colorIndex == 5)
        {
            memoryOrder.Add(greenButtonPrefab);
        }
    }
}
