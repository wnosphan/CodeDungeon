using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class CommandController : MonoBehaviour
{
    public InputField commandInputField; // Input field for entering commands
    private CharacterController characterController; // Reference to the CharacterController component

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        commandInputField.onEndEdit.AddListener(OnCommandEntered); // Add listener for command input
    }

    public void OnCommandEntered(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            // Reset player position to starting position
            characterController.ResetPlayerPosition();

            // Split the commands by new lines
            string[] commands = value.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (string command in commands)
            {
                characterController.EnqueueCommand(command.Trim()); // Add each command to the queue
            }

            commandInputField.ActivateInputField(); // Focus back to input field

            if (!characterController.IsProcessingCommands())
            {
                StartCoroutine(characterController.ProcessCommands()); // Start processing commands if not already doing so
            }
        }
    }
}
