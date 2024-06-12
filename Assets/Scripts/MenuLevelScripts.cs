using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLevelScripts : MonoBehaviour
{
    public void Level1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Level11()
    {
        SceneManager.LoadScene("Level1.1");
    }
    public void Level12()
    {
        SceneManager.LoadScene("Level1.2");
    }
    public void Level2()
    {
        SceneManager.LoadScene("Level2");
    }
    public void Level3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void Level4() => SceneManager.LoadScene("Level4");

    public void Level5() => SceneManager.LoadScene("Level5");
    
}
