using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using System;

public class CommandController : MonoBehaviour
{
    //public TMP_InputField commandInputField;
    public InputField commandInputField; // Input field for entering commands
    public Button runButton; // Reference to the run button
    private CharacterController characterController; // Reference to the CharacterController component

   
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        //commandInputField.onEndEdit.AddListener(OnCommandEntered); // Add listener for command input
        runButton.onClick.AddListener(OnRunButtonClicked); // Add listener for run button click


    }

    public void OnRunButtonClicked()
    {
        string value = commandInputField.text;
        if (!string.IsNullOrWhiteSpace(value))
        {
            OnCommandEntered(value);
        }
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
