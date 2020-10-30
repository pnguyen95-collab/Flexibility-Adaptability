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
    public GameObject winScreen;

    //pause menu
    public GameObject pauseMenu;
    private bool currentlyPaused = false;

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
    public bool memoryActive;
    public bool replicate;
    public List<GameObject> memoryOrder = new List<GameObject>();
    public List<GameObject> currentSelection = new List<GameObject>();
    private float memorizeTime;
    private float replicateTime;

    private bool startMemory = false;
    private bool startReplicate = false;
    private bool replicateTimer = false;

    //difficulty and variation progression
    private int completionAmount = 0;
    private int difficultyTier = 0;
    private bool directionSwap;

    //handles colour button indexes
    private int colourCount;
    private int colourIndex = 0;

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
        startMemory = true;
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
                    progress.ChangeProgress(0.025f);

                    //win condition check
                    if(progress.slider.value >= 1f)
                    {
                        //win screen display
                        inputScreen.SetActive(false);
                        memoryScreen.SetActive(false);
                        progressBar.SetActive(false);

                        winScreen.SetActive(true);
                        Time.timeScale = 0;
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
        }

        //if the memory game countdown hits 0, engage in the memory minigame
        if (memoryGameCountdown > 0 && startMemory == true)
        {
            memoryGameCountdown -= Time.deltaTime * 1f;
        }
        else if (memoryGameCountdown <= 0 && startMemory)
        {
            //changes bg color to a different color
            bgImage.GetComponent<Image>().color = Color.yellow;

            //turns off input minigame and activates the memory minigame
            inputActive = false;
            inputScreen.SetActive(false);
            startMemory = false;

            StartMemory();
        }

        //start of the replicate phase of memory minigame
        if (memorizeTime > 0 && startReplicate == true)
        {
            memorizeTime -= Time.deltaTime * 1f;
        }
        else if (memorizeTime <= 0 && startReplicate)
        {
            Debug.Log("ending memorize");

            //starts replicate phase
            memorizePanel.SetActive(false);
            replicatePanel.SetActive(true);
            replicateTime = 5f;
            replicateTimer = true;
            replicate = true;

            currentSelection = new List<GameObject>();
            startReplicate = false;
        }

        //timers for replicate minigame if runs out triggers minigame fail
        if (replicateTime > 0 && replicateTimer == true)
        {
            replicateTime -= Time.deltaTime * 1f;
        }
        else if (replicateTime <= 0 && replicateTimer)
        {
            Debug.Log("ending replicate");

            FailMiniGame();
        }
    }

    //function to start the input game
    public void StartInput()
    {
        Debug.Log("starting input");

        inputActive = true;
        inputScreen.SetActive(true);

        //randomises input option & updates input text accordingly
        randomIndex = UnityEngine.Random.Range(0, 26);

        inputCharacter = inputOptions[randomIndex];

        inputText.text = "Press " + inputCharacter + "!";
    }

    //function to start the memory game
    public void StartMemory()
    {
        Debug.Log("starting memory");

        memoryActive = true;

        //sets up the memorize panel with a switch based on current difficulty
        switch (difficultyTier)
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

                for(int i = 0; i < 4; i++)
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

                for (int i = 0; i < 5; i++)
                {
                    int randomColor = UnityEngine.Random.Range(0, 6);

                    CreateColorArray(randomColor);
                }

                break;
        }

        //instantiates the colour buttons
        foreach(GameObject colour in memoryOrder)
        {
            GameObject button = Instantiate(colour);
            button.transform.SetParent(memorizePanel.transform, false);
        }

        //instantiates the direction arrow
        if (directionSwap)
        {
            GameObject arrow = Instantiate(leftDirection);
            arrow.transform.SetParent(directionSpawn.transform, false);

            memorizePanel.GetComponent<HorizontalLayoutGroup>().reverseArrangement = true;
        }
        else
        {
            GameObject arrow = Instantiate(rightDirection);
            arrow.transform.SetParent(directionSpawn.transform, false);

            memorizePanel.GetComponent<HorizontalLayoutGroup>().reverseArrangement = false;
        }

        //displays the memorize panel and sets the timers
        memoryScreen.SetActive(true);
        memorizePanel.SetActive(true);
        startReplicate = true;

        //sets the amount of memorize time giving more time for harder difficulties
        switch(difficultyTier)
        {
            case 0:
                memorizeTime = 3f;
                break;
            case 1:
                memorizeTime = 3.5f;
                break;
            case 2:
                memorizeTime = 4f;
                break;
        }
    }

    //function that handles failing the memory minigame and transitions back to input minigame
    public void FailMiniGame()
    {
        Debug.Log("failed memory minigame");

        //clears all previous spawned buttons & arrows
        foreach (Transform child in memorizePanel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in directionSpawn.transform)
        {
            Destroy(child.gameObject);
        }

        //visual effects and score update
        bgImage.GetComponent<Image>().color = Color.yellow;

        tintBg.LoseTrigger();
        shake.TriggerShake(0.4f);

        progress.ChangeProgress(-0.1f);

        //changes difficulty if game has been going on for a while
        completionAmount++;

        if (completionAmount >= 3 && completionAmount <= 6) //tier 1
        {
            difficultyTier = 1;
        }
        else if (completionAmount >= 7) //tier 2
        {
            difficultyTier = 2;
        }

        //turns off memory minigame and turns back on the input minigame
        memoryOrder = new List<GameObject>();
        currentSelection = new List<GameObject>();

        memoryActive = false;
        inputActive = true;
        replicate = false;
        replicateTimer = false;

        replicatePanel.SetActive(false);
        memoryScreen.SetActive(false);

        StartInput();

        //sets up the countdown before the next memory minigame
        memoryGameCountdown = UnityEngine.Random.Range(3f, 8f);
        startMemory = true;
    }

    //function that handles winning the memory minigame and transitions back to input minigame
    public void WinMiniGame()
    {
        Debug.Log("passed memory minigame");

        //clears all previous spawned buttons & arrows
        foreach (Transform child in memorizePanel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in directionSpawn.transform)
        {
            Destroy(child.gameObject);
        }

        //visual effects and score update
        bgImage.GetComponent<Image>().color = Color.yellow;

        tintBg.WinTrigger();

        progress.ChangeProgress(0.025f);

        //win condition check
        if (progress.slider.value >= 1f)
        {
            //win screen display
            inputScreen.SetActive(false);
            memoryScreen.SetActive(false);
            progressBar.SetActive(false);

            winScreen.SetActive(true);
            Time.timeScale = 0;
        }

        //changes difficulty if game has been going on for a while
        completionAmount++;

        if (completionAmount >= 3 && completionAmount <= 6) //tier 1
        {
            difficultyTier = 1;
        }
        else if (completionAmount >= 7) //tier 2
        {
            difficultyTier = 2;
        }

        //turns off memory minigame and turns back on the input minigame
        memoryOrder = new List<GameObject>();
        currentSelection = new List<GameObject>();

        memoryActive = false;
        inputActive = true;
        replicate = false;
        replicateTimer = false;

        replicatePanel.SetActive(false);
        memoryScreen.SetActive(false);

        StartInput();

        //sets up the countdown before the next memory minigame
        memoryGameCountdown = UnityEngine.Random.Range(3f, 8f);
        startMemory = true;
    }

    //function that handles what happens when you click on the button
    public void ButtonPress(int colour)
    {
        //checks if it's currently memory minigame otherwise do nothing
        if (replicate)
        {
            colourCount = memoryOrder.Count;

            //adds the specified button to the current selection
            switch (colour)
            {
                case 0: //red
                    currentSelection.Add(redButtonPrefab);
                    break;
                case 1: //blue
                    currentSelection.Add(blueButtonPrefab);
                    break;
                case 2: //yellow
                    currentSelection.Add(yellowButtonPrefab);
                    break;
                case 3: //pink
                    currentSelection.Add(pinkButtonPrefab);
                    break;
                case 4: //teal
                    currentSelection.Add(tealButtonPrefab);
                    break;
                case 5: //green
                    currentSelection.Add(greenButtonPrefab);
                    break;
                default:
                    print("Outside Colour Range!");
                    break;
            }

            print(memoryOrder[colourIndex] + " " + currentSelection[colourIndex]);

            //compares the current index of buttons in the order and if correct keep checking until the player has gotten them all or failed on one
            if (memoryOrder[colourIndex] == currentSelection[colourIndex])
            {
                colourIndex++;
                tintBg.WinTrigger();

                //player wins minigame if all buttons pressed in correct order
                if (colourIndex == colourCount)
                {
                    WinMiniGame();
                    colourIndex = 0;
                }
            }
            else
            {
                //trigger minigame to fail
                replicate = false;
                FailMiniGame();
                colourIndex = 0;
            }
        }
 
    }

    //function to pause/unpause the game
    public void Pause()
    {
        //checks if currently paused
        if(currentlyPaused)
        {
            //unpauses the game
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            currentlyPaused = false;
        }
        else
        {
            //pause the game
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            currentlyPaused = true;
        }
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
