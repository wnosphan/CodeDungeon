using UnityEngine;
using UnityEngine.UI;

public class CompletedController : MonoBehaviour
{
    //public Text completionText; // Reference to the UI Text element for displaying the completion message
    [SerializeField] private GameObject completeScene; // Reference to the completion menu
    [SerializeField] private GameObject canVasInput;
    [SerializeField] private Button pauseBtn; // Reference to the player GameObject




    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            CompleteLevel(); // Handle level completion
        }
    }

    private void CompleteLevel()
    {

        completeScene.SetActive(true);
        canVasInput.SetActive(false);
        pauseBtn.interactable = false;
        //if (completionText != null)
        //{
        //    completionText.gameObject.SetActive(true); // Show the completion text
        //    completionText.text = "Level Completed!"; // Set the completion message text
        //}

        Debug.Log("Level Completed!"); // Log completion message
        // Add any additional logic for level completion (e.g., stop player movement, load next level, etc.)
    }
}
