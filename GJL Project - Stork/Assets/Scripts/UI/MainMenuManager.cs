using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen, loadingIcon, ContinueButton;
    [SerializeField] private AudioSource MenuAudioSFXManager;
    [SerializeField] private AudioClip buttonClick;
    AsyncOperation loadingOperation;
    private bool isLoading;
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
            }
            else
            {
                loadingIcon.SetActive(true);
                ContinueButton.SetActive(false);
            }
        }
        
    }

    public void SwitchScene()
    {
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
    }
    public void quitGame()
    {
        Screen.fullScreen = false;
        Application.Quit();
    }

    public void ButtonSFX()
    {
        MenuAudioSFXManager.PlayOneShot(buttonClick);
    }


}
