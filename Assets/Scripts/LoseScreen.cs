using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen : MonoBehaviour
{
    
    public void Respawn()
    {
        SceneManager.LoadSceneAsync(1);

    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);

    }
}
