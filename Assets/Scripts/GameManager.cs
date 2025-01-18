using UnityEngine;
using TMPro;
//using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Audio;


public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    // Game objects
    [SerializeField] private GameObject map;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject telAvivArrow;

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
    public RawImage targetLogo;
    public RawImage gameLogo;
    public RawImage background;
    public RawImage InstructionsBackground;

    // Soundtracks
    public AudioSource engineAudio;
    public AudioSource backgroundMusic;

    // Location objects
    public GameObject[] cities;

    // Manager Game state
    private GameState state = GameState.Tutorial;
    private int TutorialStage = 0;
    private int MainGameStage = 0;
    public bool isGamePaused = true;

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
        targetText.enabled = false;
        targetLogo.enabled = false;
        MissedOrHit.enabled = false;
        InstructionsBackground.enabled = false;
        instructions.enabled = false;
        controllers.enabled = false;
        UpdateGameState(GameState.Tutorial);
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
        }
        else
        {
            if (state != GameState.Tutorial)
            {
                targetLogo.enabled = true;
                targetText.enabled = true;
                MissedOrHit.enabled = true;
            }
            else if (state == GameState.Tutorial)
            {
                instructions.enabled = true;
            }
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

    }

    private void Tutorial()
    {
        switch (TutorialStage)
        {
            case 0:
                StopPlane(true);
                hitTargetScript.SetTarget("TelAviv");
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
                instructions.enabled = false;
                player.transform.position = new Vector3((float)-116.75, (float)0.1, (float)-193.3);
                player.transform.rotation = Quaternion.Euler((float)-0.79, (float)-155.37, 0);
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
}

public enum GameState
{
    Menu,
    Tutorial,
    MainGame,
    pause,
    none
}