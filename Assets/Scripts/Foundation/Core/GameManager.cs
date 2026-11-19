using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private AudioManager audioManager;

    [SerializeField] private bool isPaused;
    [SerializeField] private float pauseLayerAmount;
    [SerializeField] private float pauseLayerTarget;
    [SerializeField] private float pauseLayerSpeed = 1f;
    private InputSystem_Actions inputActions;

    public void Awake()
    {
        audioManager.GetComponent<AudioManager>(); 
        inputActions = new InputSystem_Actions();
        pauseMenu.SetActive(false);
    }

	private void OnEnable()
	{
        inputActions.UI.Pause.performed += OnPause;
		inputActions.Enable();
	}

    private void OnDisable()
    {
        inputActions.UI.Pause.performed -= OnPause;
        inputActions.Disable();
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    private void Update()
    {
        pauseLayerTarget = isPaused ? 1f : 0f;
        pauseLayerAmount = Mathf.MoveTowards(pauseLayerAmount, pauseLayerTarget, pauseLayerSpeed * Time.unscaledDeltaTime);

        audioManager.SetParameter("Pause", pauseLayerAmount);
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
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}
