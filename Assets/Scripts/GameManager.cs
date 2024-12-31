using UnityEngine;
using TMPro;
//using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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
    public TextMeshProUGUI next;
    public Image background;

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
    private void PauseGame(bool pause)
    {

        isGamePaused = pause;
        movePlayerScript.ActiveControllers = !isGamePaused;
        spawnerScript.enabled = !isGamePaused;

        // Stop all movement and rotation
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void isEnterPressedYet()
    {
        if (state == GameState.Tutorial && Input.GetKeyDown(KeyCode.Return))
        {
            TutorialStage++;
        }
        if (state == GameState.MainGame && Input.GetKeyDown(KeyCode.Return))
        {
            MainGameStage++;
            PauseGame(false);
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
                PauseGame(true);
                hitTargetScript.SetTarget("TelAviv");
                instructions.fontSize = 70;
                instructions.text = "Welcome to TargetDrop!";
                isEnterPressedYet();
                break;
            case 1:

                instructions.fontSize = 40;
                instructions.alignment = TextAlignmentOptions.Top;
                instructions.text = "Lets start with the basics!\n\r\nHow to fly?\r\nDown arrow - fly up\r\nUp  arrow - fly down\r\n" +
                    "Right arrow - turn right\r\nLeft arrow - turn left\r\nShift+Right arrow - spin right\r\nShift+Left arrow - spin Left\r\nSpace bar - drop package\r\n" +
                    "S - boost speed \r\n P - pause game";
                isEnterPressedYet();
                break;
            case 2:
                instructions.alignment = TextAlignmentOptions.Top;
                background.enabled = false;
                next.enabled = false;
                instructions.fontSize = 60;
                instructions.text = "Try dropping delivery on Tel Aviv in less then 5 km distance!";
                PauseGame(false);
                TutorialStage++;
                //UpdateGameState(GameState.Tutorialplay);
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
                player.transform.position = new Vector3((float)-116.75, (float)0.1, (float)-193.3);
                player.transform.rotation = Quaternion.Euler((float)-0.79, (float)-155.37, 0);
                    instructions.text = "Drop on : " + cities[Index_city].name;
                    hitTargetScript.SetTarget(cities[Index_city].name);
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
                instructions.text = "You hit " + distance.ToString("F2") + " km from the target, try hitting closer!";
                return;
            }
            else
            {
                instructions.text = "Good job! You Droped on the target! \r\n Press enter to begin the game!";
                telAvivArrow.SetActive(false);
                PauseGame(true);
                UpdateGameState(GameState.MainGame);
                return;
            }
        }

        if (state == GameState.MainGame)
        {
            if (distance > 5)
            {
                instructions.text = "You missed by : " + distance.ToString("F2") + " km, try hitting closer!";
                return;
            }
            else
            {
                Index_city++;
                if (Index_city < cities.Length) { 
                    hitTargetScript.SetTarget(cities[Index_city].name);
                    instructions.text = "Good job. \r\n next target: " + cities[Index_city].name;
                }
                else
                {
                    instructions.text = "Good job! you passed the first level!";
                    PauseGame(true);
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