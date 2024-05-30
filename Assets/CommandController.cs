using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CommandController : MonoBehaviour
{
    public InputField commandInputField;
    public GameObject square;
    private float moveSpeed = 1.0f; // Tốc độ di chuyển (đơn vị/giây)

    private Queue<string> commandQueue = new Queue<string>();
    private bool isProcessingCommands = false;

    void Start()
    {
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

            yield return StartCoroutine(MoveSquare(moveDirection, distance));
        }
        else
        {
            Debug.Log("Invalid command format: " + command);
        }
    }

    IEnumerator MoveSquare(Vector3 direction, int distance)
    {
        Vector3 startPosition = square.transform.position;
        Vector3 endPosition = startPosition + direction * distance;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            square.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime);
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }

        square.transform.position = endPosition; // Đảm bảo rằng đối tượng đạt được vị trí cuối cùng
    }
}
