using UnityEngine;
using System.Collections;
using TMPro;
//using UnityEngine.UIElements;

using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Audio;
using UnityEditor.Experimental.GraphView;
using System;
using Mono.Cecil.Cil;


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
    public TextMeshProUGUI instructions;
    public TextMeshProUGUI controllers;
    public TextMeshProUGUI next;
    public TextMeshProUGUI MissedOrHit;
    public TextMeshProUGUI targetText;
    public TextMeshProUGUI timerText;
    public RawImage targetLogo;
    public RawImage gameLogo;
    public RawImage background;
    public RawImage InstructionsBackground;
    public RawImage Speedometer;
    public RawImage SpeedometerP;

    // Soundtracks
    public AudioSource engineAudio;
    public AudioSource backgroundMusic;

    // Location objects
    private GameObject[] cities;
    public GameObject citiesObject;

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
    public Stopwatch watch;

    // General
    private int Index_city = 0;

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
        targetText.enabled = false;
        targetLogo.enabled = false;
        MissedOrHit.enabled = false;
        InstructionsBackground.enabled = false;
        instructions.enabled = false;
        controllers.enabled = false;
        UpdateGameState(GameState.Tutorial);

        getTargets();
        visibleTargets(false);
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
    private void ControllersMenu(bool activate)
    {
        if (activate)
        {
            miniMap.enabled = false;
            miniMapBackground.enabled = false;
            miniMapPlayer.enabled = false;
            SpeedometerP.enabled = false;
            Speedometer.enabled = false;
            targetLogo.enabled = false;
            targetText.enabled = false;
            MissedOrHit.enabled = false;
            instructions.enabled = false;
            InstructionsBackground.enabled = true;
            //instructions.enabled = true;
            //instructions.fontSize = 19;
            //instructions.alignment = TextAlignmentOptions.Top;
            controllers.enabled = true;
            //instructions.text = "Controllers:\r\nDown arrow - fly up\r\nUp  arrow - fly down\r\n" +
            //    "Right arrow - turn right\r\nLeft arrow - turn left\r\nShift+Right arrow - spin right\r\nShift+Left arrow - spin Left\r\nSpace bar - drop package\r\n" +
            //    "S - boost speed \r\n P - pause game";
            next.enabled = true;
            watch.StopStopwatch();
            watch.timerText.enabled = false;
        }
        else
        {
            if (state != GameState.Tutorial)
            {
                targetLogo.enabled = true;
                targetText.enabled = true;
                MissedOrHit.enabled = true;
                watch.StartStopwatch();
                watch.timerText.enabled = true;
            }
            else if (state == GameState.Tutorial)
            {
                instructions.enabled = true;
            }
            miniMap.enabled = true;
            miniMapBackground.enabled = true;
            miniMapPlayer.enabled = true;
            Speedometer.enabled = true;
            SpeedometerP.enabled = true;
            next.enabled = false;
            InstructionsBackground.enabled = false;
            controllers.enabled = false;
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
            //if (state == GameState.MainGame)
            //{
            ControllersMenu(true);
            //}
        }
        else
        {
            //if (state == GameState.MainGame)
            //{
            ControllersMenu(false);
            //}

        }
        //if(isGamePaused)
        //{
        //    // Print the rules
        //    instructions.text = "How to fly?\r\nDown arrow - fly up\r\nUp  arrow - fly down\r\n" +
        //            "Right arrow - turn right\r\nLeft arrow - turn left\r\nShift+Right arrow - spin right\r\nShift+Left arrow - spin Left\r\nSpace bar - drop package\r\n" +
        //            "S - boost speed \r\n P - pause game";
        //}
        //else
        //{
        //    if (state == GameState.Tutorial)
        //    {
        //        instructions.text = "Try dropping delivery on Tel Aviv in less then 5 km distance!";
        //    }
        //    else
        //    {
        //        instructions.text = "";
        //    }
        //}
    }

    private void isEnterPressedYet()
    {
        if (state == GameState.Tutorial && Input.GetKeyDown(KeyCode.Return))
        {
            TutorialStage++;
        }
        if (state == GameState.MainGame && Input.GetKeyDown(KeyCode.Return))
        {
            //targetLogo.enabled = true;
            MainGameStage++;
            StopPlane(false);
        }
        //if (planeCrash && Input.GetKeyDown(KeyCode.Return))
        //{
        //    planeCrash = false;
        //    StopPlane(false);

        //}
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

        // Check if P key was pressed to toggle pause
        if (Input.GetKeyDown(KeyCode.P))
        {
            isGamePaused = !isGamePaused; // Toggle the pause state
            PauseGame(isGamePaused);
        }

        // Stop and wait for restart
        if (planeCrash)
        {
            //StopPlane(true);
            //isEnterPressedYet();
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
                ControllersMenu(false);
                Speedometer.enabled = true;
                SpeedometerP.enabled = true;
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
        }
    }

    private void MainGame(int Index_city)
    {
        switch (MainGameStage)
        {
            case 0:
                // Wait to start game
                isEnterPressedYet();
                break;
            case 1:
                // getTargets();    
                visibleTargets(true);
                watch.StartStopwatch();
                watch.timerText.enabled = true;
                instructions.enabled = false;
                //Rigidbody rb = player.transform.GetComponent<Rigidbody>();
                //// Reset velocity and angular velocity
                //rb.linearVelocity = Vector3.zero;
                //rb.angularVelocity = Vector3.zero;
                //player.transform.position = new Vector3((float)-116.75, (float)0.1, (float)-193.3);
                //player.transform.rotation = Quaternion.Euler((float)-0.79, (float)-155.37, 0);

                movePlayerScript.respawn();

                //instructions.text = "Drop on : " + cities[Index_city].name;
                hitTargetScript.SetTarget(cities[Index_city].name);
                targetText.text = "Target: " + cities[Index_city].name;
                targetText.enabled = true;
                targetLogo.enabled = true;
                MissedOrHit.enabled = true;
                MainGameStage++;
                break;
            case 2:

                break;
        }
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
                instructions.text = "Good job! You Droped on the target! \r\n Press enter to begin the game!";
                UpdateGameState(GameState.MainGame);
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
                StartCoroutine(TimerText());
                MissedOrHit.text = "";
                Index_city++;
                if (Index_city < cities.Length)
                { 
                    hitTargetScript.SetTarget(cities[Index_city].name);
                    targetText.text = "Target: " + cities[Index_city].name;
                    //MissedOrHit.text = "Good job. \r\n next target: " + cities[Index_city].name;
                    //targetText.text = "Target: " + cities[Index_city].name;
                }
                else
                {
                    instructions.text = "Good job! you passed the first level!";
                    watch.StopStopwatch();
                    instructions.enabled = true;
                    targetText.enabled = false;
                    targetLogo.enabled = false;
                    MissedOrHit.enabled = false;
                    StopPlane(true);
                }
                return;
            }
        }
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
        if (visible) { 
            for (int i = 0; i < cities.Length; i++)
            {
                cities[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < cities.Length; i++)
            {
                cities[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private void getTargets()
    {
        //Debug.Log(citiesObject.transform.childCount);
        cities = new GameObject[citiesObject.transform.childCount];
        for (int i = 0; i < citiesObject.transform.childCount; i++)
        {
            cities[i] = citiesObject.transform.GetChild(i).gameObject;
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
        if(state == GameState.Tutorial)
        {
            instructions.enabled = true;
        }
        timerText.enabled = false;
        planeCrash = false;
    }
}

public enum GameState
{
    Menu,
    Tutorial,
    MainGame,
    //PlaneCrash,
    pause,
    none
}