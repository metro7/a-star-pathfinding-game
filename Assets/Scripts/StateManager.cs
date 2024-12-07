using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    
    public void LoseScreen()
    {
        SceneManager.LoadScene(5);
    }
}
