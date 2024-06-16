using UnityEngine;
using UnityEngine.UI;

public class CompletedController : MonoBehaviour
{
    public Text completionText; // Reference to the UI Text element for displaying the completion message

    void Start()
    {
        if (completionText != null)
        {
            completionText.gameObject.SetActive(false); // Hide the completion text at the start
        }
    }

    void Update()
    {
        // Any updates if necessary
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            CompleteLevel(); // Handle level completion
        }
    }

    private void CompleteLevel()
    {
        if (completionText != null)
        {
            completionText.gameObject.SetActive(true); // Show the completion text
            completionText.text = "Level Completed!"; // Set the completion message text
        }

        Debug.Log("Level Completed!"); // Log completion message
        // Add any additional logic for level completion (e.g., stop player movement, load next level, etc.)
    }
}
