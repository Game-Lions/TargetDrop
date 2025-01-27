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

    // Game scripts
    [SerializeField] private HitTarget hitTargetScript;
    [SerializeField] private MovePlayer movePlayerScript;
    [SerializeField] private Spawner spawnerScript;

    // Canvas game text
    public GameObject menuButtons;
    public GameObject difficultyButtons;
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
    private int ArcadeStage = 0;
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
    private int countHits = 0;  // Count the amount of cities hit in each level 
    private bool isControllersMenu = true;
    private bool isDifficultyMenu = false;
    private bool enablePauseButton = false;
    private bool enableEnterButton = true;
    private bool LevelScoreRules = true;
    private float levelAmountOfTargets;
    private float one_star;
    private float two_stars;
    private float three_stars;
    RawImage star_1_image;
    RawImage star_2_image;
    RawImage star_3_image;
    TextMeshProUGUI textComponentForLevelScore;
    //public float scaleFactor;
    private int difficulty_level;
    public float[] difficulty;

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
        difficultyButtons.SetActive(false);
        levelScore.SetActive(false);
        UpdateGameState(GameState.Tutorial);
        //levelAmountOfTargets = 0;
        //one_star = levelAmountOfTargets;
        //two_stars = levelAmountOfTargets;
        //three_stars = levelAmountOfTargets;
        star_1_image = levelScore.transform.GetChild(1).GetComponent<RawImage>();
        star_2_image = levelScore.transform.GetChild(2).GetComponent<RawImage>();
        star_3_image = levelScore.transform.GetChild(3).GetComponent<RawImage>();
        textComponentForLevelScore = levelScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
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
            case GameState.Arcade:
                break;
        }
    }
    private void Menu(bool activate) // Change name
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
            if (state == GameState.Tutorial && TutorialStage == 5)
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
            else if (isDifficultyMenu)
            {
                difficultyButtons.SetActive(true);
                backButton.SetActive(true);
                menuButtons.SetActive(false);
            }
            else
            {
                controllers.enabled = false;
                difficultyButtons.SetActive(false);
                backButton.SetActive(false);
                menuButtons.SetActive(true);
            }
        }
        else
        {
            if (state == GameState.Arcade)
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
            else if (state == GameState.FreeFly)
            {
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
            difficultyButtons.SetActive(false);
            isControllersMenu = false;
            isDifficultyMenu = false;
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
            Menu(true);
        }
        else
        {
            Menu(false);
        }
    }
    private void isEnterPressedYet()
    {
        if (enableEnterButton)
        {
            if (state == GameState.Tutorial && Input.GetKeyDown(KeyCode.Return))
            {
                TutorialStage++;
                if (TutorialStage == 5)
                {
                    PauseGame(true);
                }
            }
            if (state == GameState.Arcade && Input.GetKeyDown(KeyCode.Return))
            {
                // Called between each level
                if (ArcadeStage == 0)
                {
                    if (LevelScoreRules)
                    {
                        ArcadeStage++;
                    }
                    else
                    {
                        LevelScoreRules = true;
                    }

                }
                StopPlane(false);
                levelScore.SetActive(false);
            }
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
        if (state == GameState.Arcade)
        {
            MainGame(Index_city);
        }

        if (state == GameState.FreeFly)
        {
            FreeFly();
        }

        // Check if P key was pressed to toggle pause
        if (Input.GetKeyDown(KeyCode.P) && enablePauseButton)
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
                Menu(true);
                isEnterPressedYet();
                break;
            case 2:
                isControllersMenu = false;  // Sets to Main Menu mode
                Menu(false);
                Speedometer.enabled = true;
                SpeedometerP.enabled = true;
                Compass.enabled = true;
                next.text = "press P to \r\ncontinue";
                instructions.fontSize = 20;
                instructions.alignment = TextAlignmentOptions.Top;
                instructions.text = "Try dropping delivery on Tel Aviv\r\nin less then 5 km distance!";
                StopPlane(false);
                engineAudio.Play();
                enablePauseButton = true;
                TutorialStage++;
                break;
            case 3:
                // Here i want the code to skip othor options and wait for target hit
                break;
            case 4:
                enablePauseButton = false;
                isEnterPressedYet();    // This is the end of tutorial, it waits for enter pressed to go to the menu!
                break;
        }
    }

    private void MainGame(int Index_city)
    {
        switch (ArcadeStage)
        {
            case 0:
                // Wait to start game
                countHits = 0;
                timer.StopTimer();
                timer.timerText.enabled = false;
                telAvivArrow.SetActive(false);
                targetText.enabled = false;
                MissedOrHit.enabled = false;
                targetLogo.enabled = false;
                movePlayerScript.respawn();
                StopPlane(true);
                levelAmountOfTargets = CurrentTargets.Length;
                //one_star = levelAmountOfTargets / 3f;
                //two_stars = levelAmountOfTargets / 1.5f;
                one_star = Mathf.Ceil(levelAmountOfTargets / 3f);
                two_stars = Mathf.Ceil(levelAmountOfTargets / 1.5f);
                three_stars = levelAmountOfTargets;
                if (LevelScoreRules) // Only for first level
                {
                    float count = Level + 1;
                    star_1_image.enabled = false;
                    star_2_image.enabled = false;
                    star_3_image.enabled = false;
                    textComponentForLevelScore.text = "Level " + count +
                        "\n1 star - " + one_star + " targets" +
                        "\n2 stars - " + two_stars + " targets" +
                        "\n3 stars - " + three_stars + " targets" +
                        "\n\n You need at least 1 star to pass to the next level.";
                    textComponentForLevelScore.fontSize = 30;
                    //textComponentForLevelScore.alignment = TextAlignmentOptions.Left;
                    levelScore.SetActive(true);
                    //instructions.enabled = true;
                    //instructions.text = "Ready? press enter to begin the game!";
                }
                enableEnterButton = true;
                enablePauseButton = false;
                isEnterPressedYet();
                break;
            case 1:
                enableEnterButton = false;
                enablePauseButton = true;
                visibleTargets(true);
                setTimerByDistance();
                //instructions.enabled = false;
                levelScore.SetActive(false);
                hitTargetScript.SetTarget(CurrentTargets[Index_city].name);
                targetText.text = "Target: " + CurrentTargets[Index_city].name;
                targetText.enabled = true;
                targetLogo.enabled = true;
                MissedOrHit.enabled = true;
                ArcadeStage++;
                break;
            case 2:
                // GamePlay
                break;
        }
    }

    private void FreeFly()
    {
        enableEnterButton = false;
        enablePauseButton = true;
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
                enableEnterButton = true;
                enablePauseButton = false;
                instructions.text = "Good job! You Droped on the target! \r\n Press enter to enter the game manu";
                TutorialStage++;
                //PauseGame(true);
                //UpdateGameState(GameState.FreeFly); //  in general to menu
                telAvivArrow.SetActive(false);
                return;
            }
        }

        if (state == GameState.Arcade)
        {
            if (distance > 5)
            {
                MissedOrHit.text = "Missed by : " + distance.ToString("F2") + " km\r\n";
                StartCoroutine(TimerText("Missed target!"));
                return;
            }
            else
            {
                NextTarget(true, "Good job!");   // Toggle target hit
                return;
            }
        }
    }

    public void TimerFinished()
    {
        NextTarget(false, "Time up!");  // Toggle timer up
    }

    private void NextTarget(bool targetHit, string reason)   // If false - timer was up or plane crash, if true - target was hit
    {
        if (targetHit) { countHits++; }
        MissedOrHit.text = "";
        Index_city++;
        if (Index_city < CurrentTargets.Length)
        {
            StartCoroutine(TimerText(reason));
            hitTargetScript.SetTarget(CurrentTargets[Index_city].name);
            targetText.text = "Target: " + CurrentTargets[Index_city].name;
            setTimerByDistance();
        }
        else
        {
            bool PassedLevel = false;
            float count = Level + 1;    // Just for printing
            //textComponentForLevelScore.alignment = TextAlignmentOptions.Center;
            textComponentForLevelScore.fontSize = 40;
            //levelAmountOfTargets = CurrentTargets.Length;
            star_1_image.enabled = true;
            star_2_image.enabled = true;
            star_3_image.enabled = true;

            if (countHits < one_star)
            {
                LevelScoreRules = false;
                PassedLevel = false;
                textComponentForLevelScore.text = "You need at least 1 star for next level,\n Try again!";
                //star_1_image.enabled = true;f
                //star_2_image.enabled = true;f
                //star_3_image.enabled = true;false
                star_1_image.color = Color.black;
                star_2_image.color = Color.black;
                star_3_image.color = Color.black;
            }
            else if (countHits >= one_star && countHits < two_stars)
            {
                LevelScoreRules = false;
                PassedLevel = true;
                textComponentForLevelScore.text = "Good Job! \n you passed level " + count;
                //star_1_image.enabled = true;
                //star_2_image.enabled = false;
                //star_3_image.enabled = false;
                star_1_image.color = Color.white;
                star_2_image.color = Color.black;
                star_3_image.color = Color.black;
            }
            else if (countHits >= two_stars && countHits < three_stars)
            {
                LevelScoreRules = false;
                PassedLevel = true;
                textComponentForLevelScore.text = "Good Job! \n you passed level " + count;
                //star_1_image.enabled = true;
                //star_2_image.enabled = true;
                //star_3_image.enabled = false;
                star_1_image.color = Color.white;
                star_2_image.color = Color.white;
                star_3_image.color = Color.black;
            }
            else
            {
                LevelScoreRules = false;
                PassedLevel = true;
                textComponentForLevelScore.text = "Good Job! \n you passed level " + count + " With all stars!";
                //star_1_image.enabled = true;
                //star_2_image.enabled = true;
                //star_3_image.enabled = true;
                star_1_image.color = Color.white;
                star_2_image.color = Color.white;
                star_3_image.color = Color.white;
            }
            levelScore.SetActive(true); // Score screen
            timer.StopTimer();
            //watch.StopStopwatch();
            instructions.enabled = false;
            targetText.enabled = false;
            targetLogo.enabled = false;
            MissedOrHit.enabled = false;
            StopPlane(true);
            if (Level < Levels.Length && PassedLevel)
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
            ArcadeStage = 0;
        }
    }

    private void setTimerByDistance()
    {
        timer.timerText.enabled = true;
        float distanceFromTarget = Vector3.Distance(player.transform.position, CurrentTargets[Index_city].transform.position);

        // Apply a non-linear adjustment to the distance
        float adjustedTime = Mathf.Log(1 + distanceFromTarget) * difficulty[difficulty_level]; // Udjust scaleFactor instead

        timer.setTimer(adjustedTime); // Adjusted time based on the function
        timer.StartTimer();
        timer.timerText.enabled = true;
    }

    public void PlaneCrash()
    {
        Debug.Log("Plane crash!");
        movePlayerScript.respawn();
        if (state == GameState.Arcade) { NextTarget(false, "Plane crash!"); }
        else { StartCoroutine(TimerText("Plane crash!")); }
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

    // This function loads current target with a level
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

    IEnumerator TimerText(string text)
    {
        timerText.text = text;
        instructions.enabled = false;
        timerText.enabled = true;
        yield return new WaitForSeconds(2); // Wait for 2 seconds
        if (state == GameState.Tutorial)
        {
            instructions.enabled = true;
        }
        timerText.enabled = false;
    }
    public void FreeFlyButton()
    {

        state = GameState.FreeFly;
        movePlayerScript.respawn();
        PauseGame(false);
        Debug.Log("FreeFly pressed!");
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
        isDifficultyMenu = false;
        PauseGame(true);
        Debug.Log("Back pressed!");
    }
    public void ArcadeButton()
    {
        isDifficultyMenu = true;
        PauseGame(true);
        Debug.Log("Arcade pressed!");
    }
    public void EasyButton()
    {
        difficulty_level = 0;
        LevelScoreRules = true;
        movePlayerScript.respawn(); // Reset position
        ArcadeStage = 0;  // Reset the game stage
        Level = 0;  // Reset levels
        Index_city = 0;
        getTargets(Levels[0]);  // Set the first stage to the current targets
        state = GameState.Arcade;
        //movePlayerScript.respawn();
        PauseGame(false);
        Debug.Log("Easy pressed!");
    }
    public void MediumButton()
    {
        difficulty_level = 1;
        LevelScoreRules = true;
        movePlayerScript.respawn(); // Reset position
        ArcadeStage = 0;  // Reset the game stage
        Level = 0;  // Reset levels
        Index_city = 0;
        getTargets(Levels[0]);  // Set the first stage to the current targets
        state = GameState.Arcade;
        //movePlayerScript.respawn();
        PauseGame(false);
        Debug.Log("Medium pressed!");
    }
    public void HardButton()
    {
        difficulty_level = 2;
        LevelScoreRules = true;
        movePlayerScript.respawn(); // Reset position
        ArcadeStage = 0;  // Reset the game stage
        Level = 0;  // Reset levels
        Index_city = 0;
        getTargets(Levels[0]);  // Set the first stage to the current targets
        state = GameState.Arcade;
        //movePlayerScript.respawn();
        PauseGame(false);
        Debug.Log("Hard pressed!");
    }
    public void ExtremeButton()
    {
        difficulty_level = 3;
        LevelScoreRules = true;
        movePlayerScript.respawn(); // Reset position
        ArcadeStage = 0;  // Reset the game stage
        Level = 0;  // Reset levels
        Index_city = 0;
        getTargets(Levels[0]);  // Set the first stage to the current targets
        state = GameState.Arcade;
        //movePlayerScript.respawn();
        PauseGame(false);
        Debug.Log("Extreme pressed!");
    }

}

public enum GameState
{
    Menu,
    Tutorial,
    Arcade,
    FreeFly
}