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
    private float memorizeTime = 2f;
    private float replicateTime = 5f;
    private bool replicate;

    //difficulty and variation progression
    private int completionAmount = 0;
    private int difficultyTier = 0;
    private bool directionSwap = false;

    // Start is called before the first frame update
    void Start()
    {
        shake = cam.GetComponent<ShakeBehaviour>();
        tintBg = bgImage.GetComponent<ScreenColour>();
        progress = progressBar.GetComponent<ProgressBar>();

        StartInput();
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
}
