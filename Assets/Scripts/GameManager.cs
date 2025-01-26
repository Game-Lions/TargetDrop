//using UnityEngine;
//using System.Collections;
//using TMPro;
////using UnityEngine.UIElements;

//using UnityEngine.UI;
//using UnityEngine.InputSystem;
//using UnityEngine.UIElements;
//using UnityEngine.Audio;
//using System;
////using static UnityEngine.GraphicsBuffer;
//using System.Collections.Generic;

using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using System.Collections.Generic;
using Mono.Cecil.Cil;
//using UnityEngine.UIElements;



public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    // Game objects
    // [SerializeField] private GameObject map;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject telAvivArrow;
    [SerializeField] private GameObject telAvivText;
    private bool planeCrash = false;

    // Game scripts
    [SerializeField] private HitTarget hitTargetScript;
    [SerializeField] private MovePlayer movePlayerScript;
    [SerializeField] private Spawner spawnerScript;

    // Canvas game text
    public GameObject menuButtons;
    public GameObject backButton;
    public GameObject levelScore;
    public Button freeFlyButton;
    public Button arcadeButton;
    public Button controllersButton;
    public TextMeshProUGUI instructions;
    public TextMeshProUGUI controllers;
    public TextMeshProUGUI next;
    public TextMeshProUGUI MissedOrHit;
    public TextMeshProUGUI targetText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI muteAndPlay;
    public RawImage targetLogo;
    public RawImage gameLogo;
    public RawImage background;
    public RawImage InstructionsBackground;
    public RawImage Speedometer;
    public RawImage SpeedometerP;
    public RawImage Compass;

    // Soundtracks
    public AudioSource engineAudio;
    public AudioSource backgroundMusic;

    // Location objects
    private GameObject[] CurrentTargets;
    public GameObject[] Levels;
    private int Level = 0;

    // Manager Game state
    private GameState state = GameState.Tutorial;
    private int TutorialStage = 0;
    private int MainGameStage = 0;
    public bool isGamePaused = true;

    // MiniMap
    public RawImage miniMapBackground;
    public RawImage miniMap;
    public RawImage miniMapPlayer;

    // StopWatch
    //public Stopwatch watch;

    // Timer
    public Timer timer;

    // General
    private int Index_city = 0;
    public float scaleFactor;
    private bool isControllersMenu = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;    // Set the singleton instance to this object
            InitializeGame();
        }
    }
    private void InitializeGame()
    {
        //hitTarget = map.GetComponent<HitTarget>();
        //hitTargetScript = map.GetComponent<HitTarget>();
        //PauseGame(true);
        backgroundMusic.Play();
        miniMap.enabled = false;
        miniMapBackground.enabled = false;
        miniMapPlayer.enabled = false;
        Speedometer.enabled = false;
        SpeedometerP.enabled = false;
        Compass.enabled = false;
        targetText.enabled = false;
        targetLogo.enabled = false;
        MissedOrHit.enabled = false;
        InstructionsBackground.enabled = false;
        instructions.enabled = false;
        controllers.enabled = false;
        backButton.SetActive(false);
        muteAndPlay.enabled = false;
        menuButtons.SetActive(false);
        levelScore.SetActive(false);
        UpdateGameState(GameState.Tutorial);

        getTargets(Levels[0]);
        visibleTargets(false);

        //freeFlyButton.onClick.
    }
    public void UpdateGameState(GameState newState)
    {
        state = newState;
        switch (newState)
        {
            case GameState.Menu:
                break;
            case GameState.Tutorial:
                break;
            case GameState.MainGame:
                break;
            case GameState.pause:
                break;
        }
    }
    private void ControllersMenu(bool activate) // Change name
    {
        if (activate)
        {
            miniMap.enabled = false;
            miniMapBackground.enabled = false;
            miniMapPlayer.enabled = false;
            SpeedometerP.enabled = false;
            Speedometer.enabled = false;
            Compass.enabled = false;
            targetLogo.enabled = false;
            targetText.enabled = false;
            MissedOrHit.enabled = false;
            instructions.enabled = false;
            InstructionsBackground.enabled = true;
            muteAndPlay.enabled = true;
            timer.StopTimer();
            timer.timerText.enabled = false;
            if(state == GameState.Tutorial && TutorialStage == 5)
            {
                next.enabled = false;
            }
            else
            {
                next.enabled = true;
            }
            if (isControllersMenu)
            {
                controllers.enabled = true;
                if (state == GameState.Tutorial && TutorialStage < 2)
                {
                    backButton.SetActive(false);
                }
                else
                {
                    backButton.SetActive(true);
                }
                menuButtons.SetActive(false);
            }
            else
            {
                controllers.enabled = false;
                backButton.SetActive(false);
                menuButtons.SetActive(true);
            }
        }
        else
        {
            if (state == GameState.MainGame)
            {
                targetLogo.enabled = true;
                targetText.enabled = true;
                MissedOrHit.enabled = true;

                timer.StartTimer();
                timer.timerText.enabled = true;
                //watch.StartStopwatch();
                //watch.timerText.enabled = true;
            }
            else if (state == GameState.Tutorial)
            {
                instructions.enabled = true;
            }
            else if (state == GameState.FreeFly) {
                targetLogo.enabled = false;
                targetText.enabled = false;
                MissedOrHit.enabled = false;

                timer.StopTimer();
                timer.timerText.enabled = false;
            }
            miniMap.enabled = true;
            miniMapBackground.enabled = true;
            miniMapPlayer.enabled = true;
            Speedometer.enabled = true;
            SpeedometerP.enabled = true;
            Compass.enabled = true;
            next.enabled = false;
            InstructionsBackground.enabled = false;
            controllers.enabled = false;
            backButton.SetActive(false);
            muteAndPlay.enabled = false;
            menuButtons.SetActive(false);
            isControllersMenu = false;
            //menuButtons.SetActive(false);
        }

    }

    private void StopPlane(bool shouldStop)
    {
        isGamePaused = shouldStop;
        if (shouldStop)
        {
            engineAudio.Pause();
        }
        else
        {
            engineAudio.Play();
        }
        movePlayerScript.ActiveControllers = !isGamePaused;
        spawnerScript.enabled = !isGamePaused;
        // Stop all movement and rotation
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

    }
    private void PauseGame(bool pause)
    {
        StopPlane(pause);
        if (isGamePaused)
        {
            ControllersMenu(true);
            //if (isControllersMenu)
            //{
            //    ControllersMenu(true);
            //}
            //else
            //{
            //    ControllersMenu(false);

            //}
        }
        else
        {
            ControllersMenu(false);
        }
    }
    private void isEnterPressedYet()
    {
        if (state == GameState.Tutorial && Input.GetKeyDown(KeyCode.Return))
        {
            TutorialStage++;
            if (TutorialStage == 5)
            {
                //instructions.enabled = false;
                PauseGame(true);
            }
        }
        if (state == GameState.MainGame && Input.GetKeyDown(KeyCode.Return))
        {
            // Called between each level
            if (MainGameStage == 0)
            {
                MainGameStage++;
                
            }
            StopPlane(false);
            levelScore.SetActive(false);
        }
    }

    public void Update()
    {
        // Tutorial
        if (state == GameState.Tutorial)
        {
            Tutorial();
        }

        // Main game
        if (state == GameState.MainGame)
        {
            MainGame(Index_city);
        }

        if (state == GameState.FreeFly)
        {
            FreeFly();
        }

        // Check if P key was pressed to toggle pause
        if (Input.GetKeyDown(KeyCode.P))
        {
            isGamePaused = !isGamePaused; // Toggle the pause state
            PauseGame(isGamePaused);
        }

        // Check if M key was pressed to play and pause music
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (backgroundMusic.isPlaying)
            {
                backgroundMusic.Pause();
            }
            else
            {
                backgroundMusic.Play();
            }
        }
    }

    private void Tutorial()
    {
        switch (TutorialStage)
        {
            case 0:
                StopPlane(true);
                hitTargetScript.SetTarget("TelAviv");
                telAvivText.SetActive(true);
                instructions.fontSize = 30;
                //instructions.text = "Welcome to TargetDrop!";
                isEnterPressedYet();
                break;
            case 1:
                background.enabled = false;
                gameLogo.enabled = false;
                ControllersMenu(true);
                isEnterPressedYet();
                break;
            case 2:
                isControllersMenu = false;  // Sets to Main Menu mode
                ControllersMenu(false);
                Speedometer.enabled = true;
                SpeedometerP.enabled = true;
                Compass.enabled = true;
                next.text = "press P to \r\ncontinue";
                instructions.fontSize = 20;
                instructions.alignment = TextAlignmentOptions.Top;
                instructions.text = "Try dropping delivery on Tel Aviv\r\nin less then 5 km distance!";
                StopPlane(false);
                engineAudio.Play();
                TutorialStage++;
                break;
            case 3:
                // Here i want the code to skip othor options and wait for target hit
                break;
            case 4:
                isEnterPressedYet();    // This is the end of tutorial, it waits for enter pressed to go to the menu!
                break;
        }
    }

    private void MainGame(int Index_city)
    {
        switch (MainGameStage)
        {
            case 0:
                // Wait to start game
                //isControllersMenu = false;
                //ControllersMenu(true);
                telAvivArrow.SetActive(false);
                targetText.enabled = false;
                targetLogo.enabled = false;
                movePlayerScript.respawn();
                StopPlane(true);
                if(Level == 0)
                {
                    instructions.enabled = true;
                    instructions.text = "Ready? press enter to begin the game!";
                }
                isEnterPressedYet();
                break;
            case 1:
                visibleTargets(true);
                setTimerByDistance();

                instructions.enabled = false;
                //movePlayerScript.respawn();
                hitTargetScript.SetTarget(CurrentTargets[Index_city].name);
                targetText.text = "Target: " + CurrentTargets[Index_city].name;
                targetText.enabled = true;
                targetLogo.enabled = true;
                MissedOrHit.enabled = true;
                MainGameStage++;
                break;
            case 2:
                // GamePlay
                break;
        }
    }

    private void FreeFly()
    {
        telAvivArrow.SetActive(false);
        visibleTargets(true);
    }

    public void TargetHit(double distance)
    {
        if (state == GameState.Tutorial)
        {
            if (distance > 5)
            {
                instructions.text = "You hit " + distance.ToString("F2") + " km from the target,\r\n try hitting closer!";
                return;
            }
            else
            {
                StopPlane(true);    
                instructions.text = "Good job! You Droped on the target! \r\n Press enter to enter the game manu";
                TutorialStage++;
                //PauseGame(true);
                //UpdateGameState(GameState.FreeFly); //  in general to menu
                telAvivArrow.SetActive(false);
                return;
            }
        }

        if (state == GameState.MainGame)
        {
            if (distance > 5)
            {
                MissedOrHit.text = "Missed by : " + distance.ToString("F2") + " km\r\n";
                return;
            }
            else
            {
                //
                MissedOrHit.text = "";
                Index_city++;
                if (Index_city < CurrentTargets.Length)
                {
                    StartCoroutine(TimerText());
                    hitTargetScript.SetTarget(CurrentTargets[Index_city].name);
                    targetText.text = "Target: " + CurrentTargets[Index_city].name;
                    setTimerByDistance();
                    //MissedOrHit.text = "Good job. \r\n next target: " + cities[Index_city].name;
                    //targetText.text = "Target: " + cities[Index_city].name;
                }
                else
                {
                    float count = Level + 1;
                    //instructions.text = "Good job! you passed level " + count + "\n Press enter for next level";
                    levelScore.SetActive(true);
                    timer.StopTimer();
                    //watch.StopStopwatch();
                    instructions.enabled = false;
                    targetText.enabled = false;
                    targetLogo.enabled = false;
                    MissedOrHit.enabled = false;
                    StopPlane(true);
                    if (Level < Levels.Length)
                    {
                        Level++;
                    }
                    else
                    {
                        instructions.text = "Congradulations! you Passed all levels";
                    }
                    Index_city = 0;
                    // Add if statment to go to next level or not
                    getTargets(Levels[Level]);
                    MainGameStage = 0;
                    //isEnterPressedYet();
                }
                return;
            }
        }
    }

    private void setTimerByDistance()
    {
        timer.timerText.enabled = true;
        float distanceFromTarget = Vector3.Distance(player.transform.position, CurrentTargets[Index_city].transform.position);

        // Apply a non-linear adjustment to the distance
        float adjustedTime = Mathf.Log(1 + distanceFromTarget) * scaleFactor; // Udjust scaleFactor instead

        timer.setTimer(adjustedTime); // Adjusted time based on the function
        timer.StartTimer();
        timer.timerText.enabled = true;
    }

    public void PlaneCrash()
    {
        Debug.Log("Plane crash!");
        planeCrash = true;
        StartCoroutine(TimerText());
        movePlayerScript.respawn();
    }

    private void visibleTargets(bool visible)
    {
        if (visible)
        {
            for (int i = 0; i < Levels.Length; i++)
            {
                for (int j = 0; j < Levels[i].transform.childCount; j++)
                {
                    GameObject child = Levels[i].transform.GetChild(j).gameObject;
                    child.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                }
            }
        }
        else
        {
            for (int i = 0; i < Levels.Length; i++)
            {
                for (int j = 0; j < Levels[i].transform.childCount; j++)
                {
                    GameObject child = Levels[i].transform.GetChild(j).gameObject;
                    child.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                }
            }
        }
    }

    private void getTargets(GameObject level)
    {
        int childCount = level.transform.childCount;
        CurrentTargets = new GameObject[childCount];

        // Create a list of indices
        List<int> indices = new List<int>();
        for (int i = 0; i < childCount; i++)
        {
            indices.Add(i);
        }

        // Shuffle the indices
        for (int i = 0; i < indices.Count; i++)
        {
            int randomIndex = Random.Range(i, indices.Count);
            // Swap the elements
            int temp = indices[i];
            indices[i] = indices[randomIndex];
            indices[randomIndex] = temp;
        }

        // Assign cities in random order
        for (int i = 0; i < childCount; i++)
        {
            CurrentTargets[i] = level.transform.GetChild(indices[i]).gameObject;
        }
    }


    IEnumerator TimerText()
    {
        if (planeCrash)
        {
            timerText.text = "Plane Crash!\nTry again";
            instructions.enabled = false;
            timerText.enabled = true;
        }
        else
        {
            timerText.text = "Good Job!";
            timerText.enabled = true;
        }
        yield return new WaitForSeconds(2); // Wait for 2 seconds
        if (state == GameState.Tutorial)
        {
            instructions.enabled = true;
        }
        timerText.enabled = false;
        planeCrash = false;
    }
    public void FreeFlyButton()
    {
        state = GameState.FreeFly;
        movePlayerScript.respawn();
        PauseGame(false);
        Debug.Log("FreeFly pressed!");
    }
    public void ArcadeButton()
    {
        state = GameState.MainGame;
        //movePlayerScript.respawn();
        PauseGame(false);
        Debug.Log("Arcade pressed!");
    }
    public void ControllersButton()
    {
        isControllersMenu = true;
        PauseGame(true);
        Debug.Log("Controllers pressed!");
    }
    public void BackButton()
    {
        isControllersMenu = false;
        PauseGame(true);
        Debug.Log("Back pressed!");
    }
}

public enum GameState
{
    Menu,
    Tutorial,
    MainGame,
    FreeFly,
    //PlaneCrash,
    pause,
    none
}