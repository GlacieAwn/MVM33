using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private AudioManager audioManager;

    private bool isPaused;

    public void Awake()
    {
        audioManager.GetComponent<AudioManager>();
    }

    public void TogglePause()
    {
        if(isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        audioManager.SetParameter("Pause", 1f);
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        audioManager.SetParameter("Pause", 0f);
    }
}
