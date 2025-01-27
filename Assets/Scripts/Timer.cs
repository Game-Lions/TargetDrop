using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Assign a UI Text in the Inspector
    private float duration; // Set the timer duration in seconds in the Inspector
    private float remainingTime;
    private bool isRunning = false;
    private

    //void Start()
    //{
    //    remainingTime = duration;
    //    UpdateTimerUI();
    //}

    void Update()
    {
        if (isRunning && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                remainingTime = 0;
                isRunning = false;
                OnTimerEnd(); // Call this when the timer ends
            }
            UpdateTimerUI();
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        remainingTime = duration;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        int milliseconds = Mathf.FloorToInt((remainingTime * 1000) % 100);
        timerText.text = $"{minutes:D2}:{seconds:D2}:{milliseconds:D2}";
        if (minutes < 1 && seconds < 7)
        {
            timerText.color = Color.red;
        }
        else
        {
            timerText.color = new Color32(230, 169, 98, 255); // E6A962 in RGBA
        }
    }

    public void setTimer(float time)
    {
        duration = time;
        remainingTime = duration;
        UpdateTimerUI();
    }

    private void OnTimerEnd()
    {
        Debug.Log("Timer finished!");
        GameManager.instance.TimerFinished();
        // Add additional logic here (e.g., show a message or trigger an event).
    }
}
