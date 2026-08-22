using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject audioManager;

    private bool isPaused;

    public void TogglePause()
    {
        if(isPaused)
        {
            // Resume Game Here
        }
        else
        {
            // Pause game Here
        }
    }

    public void Pause()
    {
        
    }

    public void Resume()
    {
        
    }
}
