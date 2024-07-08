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
    public CharacterController characterController; // Reference to the CharacterController component
    public MonsterController monsterController; // Reference to the MonsterController component

    void Start()
    {
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
            if (characterController == null)
            {
                Debug.LogError("CharacterController component is not assigned and not found on the same GameObject.");
            }
        }

        if (monsterController == null)
        {
            // Find the MonsterController component in the scene
            monsterController = FindObjectOfType<MonsterController>();
            if (monsterController == null)
            {
                Debug.LogError("MonsterController component is not assigned and not found in the scene.");
            }
        }

        // Add listener for run button click
        runButton.onClick.AddListener(OnRunButtonClicked);
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
            if (characterController != null)
            {
                characterController.ResetPlayerPosition();
            }

            if (monsterController != null)
            {
                monsterController.ResetPosition();
            }

            // Split the commands by new lines
            string[] commands = value.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (string command in commands)
            {
                if (characterController != null)
                {
                    characterController.EnqueueCommand(command.Trim()); // Add each command to the queue
                }
            }

            commandInputField.ActivateInputField(); // Focus back to input field

            if (characterController != null && !characterController.IsProcessingCommands())
            {
                StartCoroutine(characterController.ProcessCommands()); // Start processing commands if not already doing so
            }
        }
    }
}
