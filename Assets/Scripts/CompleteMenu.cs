using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteMenu : MonoBehaviour
{
   
    public void Home()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1;
    }

    public void Restart()
    {
        // Get the active scene name
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Reload the active scene
        SceneManager.LoadScene(currentSceneName);

        // Reset the time scale to normal
        Time.timeScale = 1;

    }
}
