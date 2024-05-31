using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class CommandController : MonoBehaviour
{
    public InputField commandInputField;
    public GameObject player;
    private float moveSpeed = 1.0f; // Movement speed (units/second)
    private Queue<string> commandQueue = new Queue<string>();
    private bool isProcessingCommands = false;
    private Animator animator;
    public LayerMask solidObjectLayer;
 
    void Start()
    {
        if (player != null)
        {
            animator = player.GetComponent<Animator>();
        }

        commandInputField.onEndEdit.AddListener(OnCommandEntered);
    }

    public void OnCommandEntered(string value)
    {
        // Split the commands by new lines
        string[] commands = value.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string command in commands)
        {
            commandQueue.Enqueue(command.Trim()); // Add each command to the queue
        }

        commandInputField.ActivateInputField(); // Focus back to input field
        commandInputField.text = string.Empty; // Clear input field after entering command

        if (!isProcessingCommands)
        {
            StartCoroutine(ProcessCommands());
        }
    }

    IEnumerator ProcessCommands()
    {
        isProcessingCommands = true;

        while (commandQueue.Count > 0)
        {
            string command = commandQueue.Dequeue();
            yield return StartCoroutine(ProcessCommand(command));
        }

        isProcessingCommands = false;
    }

    IEnumerator ProcessCommand(string command)
    {
        // Extract the direction and distance from the command using regex
        var match = Regex.Match(command, @"(\w+)\((\d+)\)");
        if (match.Success)
        {
            string direction = match.Groups[1].Value.ToLower();
            int distance = int.Parse(match.Groups[2].Value);

            Vector3 moveDirection = Vector3.zero;
            switch (direction)
            {
                case "up":
                    moveDirection = Vector3.up;
                    break;
                case "down":
                    moveDirection = Vector3.down;
                    break;
                case "left":
                    moveDirection = Vector3.left;
                    break;
                case "right":
                    moveDirection = Vector3.right;
                    break;
                default:
                    Debug.Log("Unknown command: " + command);
                    yield break;
            }

            yield return StartCoroutine(MovePlayer(moveDirection, distance));
        }
        else
        {
            Debug.Log("Invalid command format: " + command);
        }
    }

    IEnumerator MovePlayer(Vector3 direction, int distance)
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
        }

        // Move the player to the target position
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
                // Move back one unit if the position is not walkable
                player.transform.position -= direction;
                break;
            }

            // Move the player to the current position
            player.transform.position = currentPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset animator parameters after movement
        if (animator != null)
        {
            animator.SetBool("isMoving", false);
            // Do not reset moveX and moveY to keep the direction
        }
    }

    //IEnumerator MovePlayer(Vector3 direction, int distance)
    //{
    //    if (player == null)
    //    {
    //        Debug.LogWarning("Player GameObject is null or has been destroyed.");
    //        yield break;
    //    }

    //    Vector3 startPosition = player.transform.position;
    //    Vector3 endPosition = startPosition + direction * distance;
    //    float totalDuration = distance / moveSpeed;
    //    float elapsedTime = 0f;

    //    // Set animator parameters to start the animation
    //    if (animator != null)
    //    {
    //        animator.SetBool("isMoving", true);
    //        animator.SetFloat("moveX", direction.x);
    //        animator.SetFloat("moveY", direction.y);
    //    }

    //    while (elapsedTime < totalDuration)
    //    {
    //        if (player == null)
    //        {
    //            Debug.LogWarning("Player GameObject is null or has been destroyed during movement.");
    //            yield break;
    //        }

    //        // Calculate the current position based on elapsed time
    //        float t = elapsedTime / totalDuration;
    //        Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, t);

    //        // Check if the current position is walkable
    //        if (!IsWalkable(currentPosition))
    //        {

    //            Debug.LogWarning("Current position is not walkable: " + currentPosition);
    //            break;
    //        }

    //        // Move the player to the current position
    //        player.transform.position = currentPosition;

    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    // Reset animator parameters after movement
    //    if (animator != null)
    //    {
    //        animator.SetBool("isMoving", false);
    //        // Do not reset moveX and moveY to keep the direction
    //    }
    //}

    //set animator parameters to stop the animation
    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectLayer) != null)
        {
            return false;
        }
        return true;
    }
}
