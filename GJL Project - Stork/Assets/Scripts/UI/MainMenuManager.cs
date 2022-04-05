using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen, loadingIcon, ContinueButton, mainMenuFirstButton;
    [SerializeField] private AudioSource MenuAudioSFXManager;
    [SerializeField] private AudioClip buttonClick;
    AsyncOperation loadingOperation;
    private bool isLoading;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainMenuFirstButton);
    }
    public void LoadGame()
    {
        loadingScreen.SetActive(true);
        loadingOperation = SceneManager.LoadSceneAsync("GameScene");
        loadingOperation.allowSceneActivation = false;
        StartCoroutine("TransitionTimer");
        
    }

    private void Update()
    {
        if (isLoading)
        {
            if (loadingOperation.progress >= 0.9f)
            {
                loadingIcon.SetActive(false);
                ContinueButton.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(ContinueButton);
            }
        }
        
    }

    public void SwitchScene()
    {
        isLoading = false;
        loadingOperation.allowSceneActivation = true;
    }

    public IEnumerator TransitionTimer()
    {
        loadingIcon.SetActive(true);
        ContinueButton.SetActive(false);
        yield return new WaitForSeconds(2);
        isLoading = true;
    }
    public void FullscreenToggle(bool status)
    {
        Screen.fullScreen = status;
        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
    }
    public void quitGame()
    {
        Screen.fullScreen = false;
        Cursor.lockState = CursorLockMode.None;
        if(Application.platform != RuntimePlatform.WebGLPlayer)
        {
            Application.Quit();
        }
        
    }

    public void ButtonSFX()
    {
        MenuAudioSFXManager.PlayOneShot(buttonClick);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("GameScene");
    }


}
