using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool gameRuning, isPaused;

    private float timer;
    [SerializeField] private float gameDuration;

    public delegate void GameEvent();
    public static event GameEvent OnStarted;
    public static event GameEvent OnComplete;
    public static event GameEvent OnPause;
    public static event GameEvent OnUnpause;

    //private void Start()
    //{
    //    StartGame();
    //}
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
            if(timer <= 0)
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
}
