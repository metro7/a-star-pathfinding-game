using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void InstructionsMenu()
    {
        SceneManager.LoadSceneAsync(4);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

}
