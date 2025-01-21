using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;



public class Stopwatch : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Assign a UI Text in the Inspector
    private float elapsedTime = 0f;
    private bool isRunning = false;

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    public void StartStopwatch()
    {
        isRunning = true;
    }

    public void StopStopwatch()
    {
        isRunning = false;
    }

    public void ResetStopwatch()
    {
        elapsedTime = 0f;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 1000) % 100);
        timerText.text = $"{minutes:D2}:{seconds:D2}:{milliseconds:D2}";
    }
}
