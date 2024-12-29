using UnityEngine;
using TMPro;
//using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public GameState state;

    public static GameManager instance; // Singleton instance
    //public GameObject map;
    public GameObject terrain;
    private HitTarget hitTarget;
    public GameObject player;
    [SerializeField] private MovePlayer movePlayerScript;
    private MovePlayer movePlayer;
    public GameObject arrow;
    public TextMeshProUGUI instructions;
    public TextMeshProUGUI next;
    public Image background;


    public GameObject[] EasyLocations;
    public GameObject[] cities;

    public bool isGamePaused = true; // Flag to control the pause state
    private int TutorialStage = 0;
    private int MainGameStage = 0;
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
        hitTarget = terrain.GetComponent<HitTarget>();
        UpdateGameState(GameState.TutorialInstructions);

    }
    public void UpdateGameState(GameState newState)
    {
        state = newState;
        switch (newState)
        {
            case GameState.Menu:
                break;
            case GameState.TutorialInstructions:
                arrow.SetActive(true);
                hitTarget.SetTarget("TelAviv");
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
    }

    private void isEnterPressedYet()
    {
        if (state == GameState.TutorialInstructions && Input.GetKeyDown(KeyCode.Return))
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
        if (state == GameState.TutorialInstructions)
        {
            Tutorial();
            isEnterPressedYet();
        }

        //if (state == GameState.Tutorialplay)
        //{
        //}

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

    public void Tutorial()
    {
        switch (TutorialStage)
        {
            case 0:
                PauseGame(true);
                instructions.fontSize = 70;
                instructions.text = "Welcome to TargetDrop!";
                break;
            case 1:

                instructions.fontSize = 40;
                instructions.alignment = TextAlignmentOptions.Top;
                instructions.text = "Lets start with the basics!\n\r\nHow to fly?\r\nDown arrow - fly up\r\nUp  arrow - fly down\r\nRight arrow - turn right\r\nLeft arrow - turn left\r\nShift+Right arrow - spin right\r\nShift+Left arrow - spin Left\r\nSpace bar - drop package";
                break;
            case 2:
                instructions.alignment = TextAlignmentOptions.Top;
                background.enabled = false;
                next.enabled = false;
                instructions.fontSize = 60;
                instructions.text = "Try dropping delivery on Tel Aviv in less then 5 km distance!";
                PauseGame(false);
                UpdateGameState(GameState.Tutorialplay);
                break;
        }
    }

    public void MainGame(int city)
    {
        switch (MainGameStage)
        {
            case 0:
                isEnterPressedYet();
                break;
            case 1:
                player.transform.position = new Vector3(-135, 9, -268);
                instructions.text = "Hit " + cities[city].name;
                hitTarget.SetTarget(cities[city].name);
                //UpdateGameState(GameState.Tutorialplay);
                MainGameStage++;
                break;
            case 2:

                break;
        }
    }

    public void TargetHit(double distance)
    {
        if (state == GameState.Tutorialplay)
        {
            if (distance > 5)
            {
                instructions.text = "You hit " + distance.ToString("F2") + " km from the target, try hitting closer!";
            }
            else
            {
                instructions.text = "Good job! You hit the target! \r\n Press enter to begin the game!";
                PauseGame(true);
                UpdateGameState(GameState.MainGame);
            }
        }

        if (state == GameState.MainGame)
        {
            if (distance > 5)
            {
                instructions.text = "You hit " + distance.ToString("F2") + " km from the target, try hitting closer!";
            }
            else
            {
                Index_city++;
                hitTarget.SetTarget(cities[Index_city].name);
                instructions.text = "Good job. next target: "+ cities[Index_city].name;
            }
        }


    }
}

public enum GameState
{
    Menu,
    TutorialInstructions,
    Tutorialplay,
    MainGame,
    pause,
    none
}