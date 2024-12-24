using UnityEngine;
using TMPro;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameState state;

    public static GameManager instance; // Singleton instance
    public GameObject map;
    private HitTarget hitTarget;
    public GameObject player;
    [SerializeField] private MovePlayer movePlayerScript;
    private MovePlayer movePlayer;
    public GameObject arrow;
    public TextMeshProUGUI instructions;
    public TextMeshProUGUI next;
    public Image background;

    public bool isGamePaused = true; // Flag to control the pause state
    private int TutorialStage = 0;

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
        hitTarget = map.GetComponent<HitTarget>();
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
        movePlayerScript.ActiveControllers = !pause;
    }

    private void isEnterPressedYet()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TutorialStage++;
        }
    }

    public void Update()
    {
        if (state == GameState.TutorialInstructions)
        {
            Tutorial();
            isEnterPressedYet();
        }
        //if (state == GameState.Tutorialplay)
        //{
        //    isEnterPressedYet();
        //}
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
                instructions.text = "Try droping delivery on Tel Aviv in less then 5 km distance!";
                PauseGame(false);
                UpdateGameState(GameState.Tutorialplay);
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
                instructions.text = "Good job! You hit " + distance.ToString("F2") + " km from the target! \r\n Lets begin the game!";
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