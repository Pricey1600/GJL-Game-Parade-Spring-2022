using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private bool gameRuning, isPaused;
    [SerializeField] private bool mainMenu;

    private float timer;
    [SerializeField] private float gameDuration;

    public delegate void GameEvent();
    public static event GameEvent OnStarted;
    public static event GameEvent OnComplete;
    public static event GameEvent OnPause;
    public static event GameEvent OnUnpause;

    [SerializeField] private TMP_Text timerText;
    private string minutes, seconds;

    [SerializeField] private GameObject DesktopControls, ControllerControls;

    private void Start()
    {
        if (!mainMenu)
        {
            StartGame();
        }
        
    }
    public void StartGame()
    {
        gameRuning = true;
        OnStarted?.Invoke();
        timer = gameDuration;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            //update UI
            minutes = Mathf.Floor(timer / 60).ToString("0");
            seconds = Mathf.Floor(timer % 60).ToString("00");
            timerText.text = minutes + ":" + seconds;
            if (timer <= 0)
            {
                //game over. Trigger event
                gameRuning = false;
                Cursor.lockState = CursorLockMode.Confined;
                OnComplete?.Invoke();
            }
        }
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            isPaused = true;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0f;
            OnPause?.Invoke();
        }
        else
        {
            isPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
            OnUnpause?.Invoke();
        }
        
    }

    public void UpdateControls(PlayerInput pi)
    {
        if(DesktopControls != null && ControllerControls != null)
        {
            if (pi.currentControlScheme.Equals("Controller") == true)
            {
                DesktopControls.SetActive(false);
                ControllerControls.SetActive(true);
            }
            else
            {
                DesktopControls.SetActive(true);
                ControllerControls.SetActive(false);
            }
        }
        
    }
}
