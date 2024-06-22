using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CharacterController: MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject
    public LayerMask solidObjectLayer; // Layer mask for solid objects
    public LayerMask outDoorLayer; // Layer mask for doors
    private Queue<string> commandQueue = new Queue<string>(); // Queue to store commands
    private bool isProcessingCommands = false; // Flag to indicate if commands are being processed
    private Animator animator; // Animator component of the player
    private Rigidbody2D playerRigidbody; // Rigidbody2D component of the player
    private CapsuleCollider2D playerCapsuleCollider; // Collider component of the player
    private Vector3 startPoint; // Starting position of the player
    [SerializeField] private float moveSpeed = 1.0f; // Movement speed (units/second)
    [SerializeField] private float contactDistance = 0.1f; // Distance to check for collisions

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
        

    void Start()
    {
        if (player != null)
        {
            animator = player.GetComponent<Animator>(); // Get Animator component from player
            playerRigidbody = player.GetComponent<Rigidbody2D>(); // Get Rigidbody2D component from player
            playerCapsuleCollider = player.GetComponent<CapsuleCollider2D>(); // Get Collider component from player
            startPoint = player.transform.position; // Store the starting position of the player
        }
    }

    public void EnqueueCommand(string command)
    {
        commandQueue.Enqueue(command);
    }

    public IEnumerator ProcessCommands()
    {
        isProcessingCommands = true;

        while (commandQueue.Count > 0)
        {
            string command = commandQueue.Dequeue();
            yield return StartCoroutine(ProcessCommand(command)); // Process each command
        }

        isProcessingCommands = false;
    }

    private IEnumerator ProcessCommand(string command)
    {
        // Extract the direction and distance from the command using regex
        var match = Regex.Match(command, @"(\w+)\((\d+(\.\d+)?)\)");
        if (match.Success)
        {
            string direction = match.Groups[1].Value.ToLower();
            float distance = float.Parse(match.Groups[2].Value);

            Vector3 moveDirection = GetDirectionVector(direction); // Get the direction vector
            if (moveDirection == Vector3.zero)
            {
                Debug.Log("Unknown command: " + command);
                yield break;
            }

            yield return StartCoroutine(MovePlayer(moveDirection, distance)); // Move the player
        }
        else
        {
            Debug.Log("Invalid command format: " + command);
        }
    }

    private Vector3 GetDirectionVector(string direction)
    {
        switch (direction)
        {
            case "up": return Vector3.up;
            case "down": return Vector3.down;
            case "left": return Vector3.left;
            case "right": return Vector3.right;
            default: return Vector3.zero;
        }
    }

    private IEnumerator MovePlayer(Vector3 direction, float distance)
    {
        if (player == null)
        {
            Debug.LogWarning("Player GameObject is null or has been destroyed.");
            yield break;
        }

        Vector3 startPosition = player.transform.position;
        Vector3 endPosition = startPosition + direction * distance;
        float totalDuration = distance / moveSpeed;
        float elapsedTime = 0f;

        // Set animator parameters to start the animation
        if (animator != null)
        {
            animator.SetBool("isMoving", true);
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);

            // Play the walk sound effect
            audioManager.PlaySfx(audioManager.walkClip);

        }

        while (elapsedTime < totalDuration)
        {
            if (player == null)
            {
                Debug.LogWarning("Player GameObject is null or has been destroyed during movement.");
                yield break;
            }

            // Calculate the current position based on elapsed time
            float t = elapsedTime / totalDuration;
            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, t);

            // Check if the current position is walkable
            if (!IsWalkable(currentPosition))
            {
                Debug.LogWarning("Current position is not walkable: " + currentPosition);
                break; // Stop movement when hitting a solid object
            }

            // Check if the player meets the outdoor layer
            if (IsOutDoor(currentPosition))
            {
                Debug.LogWarning("Current position is Door: " + currentPosition);
                break; // Stop movement when meeting outdoor layer
            }

            // Move the player to the current position using Rigidbody2D
            playerRigidbody.MovePosition(currentPosition);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset animator parameters after movement
        if (animator != null)
        {
            animator.SetBool("isMoving", false);

            // Stop the walk sound effect
            
        }
    }

    //private bool IsWalkable(Vector3 targetPos)
    //{
    //    // Use CapsuleCollider2D to check for collisions
    //    Vector2 direction = (targetPos - player.transform.position).normalized;
    //    float distance = Vector2.Distance(player.transform.position, targetPos);
    //    RaycastHit2D hit = Physics2D.CapsuleCast(player.transform.position, playerCapsuleCollider.size, playerCapsuleCollider.direction, 0f, direction, distance, solidObjectLayer);

    //    return hit.collider == null;
    //}

    //private bool IsOutDoor(Vector3 targetPos)
    //{
    //    // Use CapsuleCollider2D to check for collisions with outdoor layer
    //    Vector2 direction = (targetPos - player.transform.position).normalized;
    //    float distance = Vector2.Distance(player.transform.position, targetPos);
    //    RaycastHit2D hit = Physics2D.CapsuleCast(player.transform.position, playerCapsuleCollider.size, playerCapsuleCollider.direction, 0f, direction, distance, outDoorLayer);

    //    return hit.collider != null;
    //}

    private bool IsWalkable(Vector3 targetPos)
    {
        // Use CapsuleCollider2D to check for collisions
        Vector2 direction = (targetPos - player.transform.position).normalized;
        float distance = Vector2.Distance(player.transform.position, targetPos) + contactDistance;
        RaycastHit2D hit = Physics2D.CapsuleCast(player.transform.position, playerCapsuleCollider.size, playerCapsuleCollider.direction, 0f, direction, distance, solidObjectLayer);

        return hit.collider == null;
    }

    private bool IsOutDoor(Vector3 targetPos)
    {
        // Use CapsuleCollider2D to check for collisions with outdoor layer
        Vector2 direction = (targetPos - player.transform.position).normalized;
        float distance = Vector2.Distance(player.transform.position, targetPos) + contactDistance;
        RaycastHit2D hit = Physics2D.CapsuleCast(player.transform.position, playerCapsuleCollider.size, playerCapsuleCollider.direction, 0f, direction, distance, outDoorLayer);

        return hit.collider != null;
    }

    public void ResetPlayerPosition()
    {
        player.transform.position = startPoint;
    }

    public bool IsProcessingCommands()
    {
        return isProcessingCommands;
    }
}
