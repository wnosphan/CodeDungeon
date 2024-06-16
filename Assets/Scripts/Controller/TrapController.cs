using UnityEngine;
using UnityEngine.UI;

public class TrapController : MonoBehaviour
{
    public Text failureText; // Reference to the UI Text element for displaying the failure message
    private Vector3 startPosition; // The starting position of the player

    void Start()
    {
        // Store the starting position of the player
        startPosition = transform.position;

        if (failureText != null)
        {
            failureText.gameObject.SetActive(false); // Hide the failure text at the start
        }
    }

    void Update()
    {
        // Any updates if necessary
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            HandleTrap(); // Handle trap collision and respawn
        }
    }

    private void HandleTrap()
    {
        if (failureText != null)
        {
            failureText.gameObject.SetActive(true); // Show the failure text
            failureText.text = "You Failed! Respawning..."; // Set the failure message text
        }

        Debug.Log("You Failed! Respawning..."); // Log failure message

        // Respawn the player to the starting position
        transform.position = startPosition;
    }
}
