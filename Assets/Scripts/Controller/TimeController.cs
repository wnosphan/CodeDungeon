using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public Text txtTime; // Reference to the Text component to display time
    public Button runButton; // Reference to the Run button
    public GameObject player; // Reference to the player GameObject
    private float elapsedTime = 0f; // Elapsed time in seconds
    private bool isRunning = false; // Flag to check if the timer is running

    void Start()
    {
        // Ensure the Text and Button are assigned
        if (txtTime == null)
        {
            Debug.LogError("txtTime is not assigned. Please assign it in the Inspector.");
        }

        if (runButton == null)
        {
            Debug.LogError("Run Button is not assigned. Please assign it in the Inspector.");
        }

        // Add listener to the Run button
        runButton.onClick.AddListener(OnRunButtonClicked);
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimeDisplay();
        }
    }

    void OnRunButtonClicked()
    {
        // Start the timer when the button is clicked
        isRunning = true;
        elapsedTime = 0f; // Reset elapsed time
    }

    void UpdateTimeDisplay()
    {
        // Format the elapsed time to display as minutes:seconds:milliseconds
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        float milliseconds = (elapsedTime * 1000f) % 1000f;
        txtTime.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}
