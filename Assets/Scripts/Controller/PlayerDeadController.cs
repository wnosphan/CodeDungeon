using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeadController : MonoBehaviour
{
    [SerializeField] private GameObject deadScene; // Reference to the completion menu
    [SerializeField] private GameObject canVasInput;
    [SerializeField] private Button pauseBtn; // Reference to the player GameObject
    private TimeController timeController; // Reference to the TimeController script
                                           // Start is called before the first frame update
    void Start()
    {
        // Find the TimeController script in the scene
        timeController = FindObjectOfType<TimeController>();
        if (timeController == null)
        {
            Debug.LogError("TimeController script not found in the scene.");
        }

        // Ensure the other references are assigned
        if (deadScene == null)
        {
            Debug.LogError("completeScene is not assigned. Please assign it in the Inspector.");
        }

        if (canVasInput == null)
        {
            Debug.LogError("canVasInput is not assigned. Please assign it in the Inspector.");
        }

        if (pauseBtn == null)
        {
            Debug.LogError("pauseBtn is not assigned. Please assign it in the Inspector.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            CompleteLevel(); // Handle level completion
        }
    }

    private void CompleteLevel()
    {
        if (timeController != null)
        {
            timeController.StopTimer();
        }

        if (deadScene != null)
        {
            deadScene.SetActive(true);
        }

        if (canVasInput != null)
        {
            canVasInput.SetActive(false);
        }

        if (pauseBtn != null)
        {
            pauseBtn.interactable = false;
        }
        Time.timeScale = 0;
        Debug.Log("Player Dead!"); // Log completion message
        // Add any additional logic for level completion (e.g., stop player movement, load next level, etc.)
    }
}
