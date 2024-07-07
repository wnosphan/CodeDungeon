using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Game Close");
    }
    public void SelectLevel()
    {
        SceneManager.LoadScene("LevelMenu");
    }

    public void RankingButton()
    {
        SceneManager.LoadScene("Ranking");
    }

}
