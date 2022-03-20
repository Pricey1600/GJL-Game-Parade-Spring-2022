using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private int score, missed;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject gameCompleteScreen;

    [SerializeField] private AudioSource nonDiegeticSource;
    [SerializeField] private AudioClip successfulSFX, failSFX, gameComplete;

    private void OnEnable()
    {
        BabyScript.OnSuccess += AddToScore;
        BabyScript.OnFailure += AddToMissed;

        GameManager.OnStarted += ResetScores;
        GameManager.OnComplete += UpdateUI;

        
    }

    private void OnDisable()
    {
        BabyScript.OnSuccess -= AddToScore;
        BabyScript.OnFailure -= AddToMissed;

        GameManager.OnStarted -= ResetScores;
        GameManager.OnComplete -= UpdateUI;
    }

    private void Start()
    {
        gameCompleteScreen.SetActive(false);
    }

    private void AddToScore()
    {
        score++;
        nonDiegeticSource.PlayOneShot(successfulSFX);
    }

    private void AddToMissed()
    {
        missed++;
        nonDiegeticSource.PlayOneShot(failSFX);
    }

    private void UpdateUI()
    {
        //update score UI on game complete screen
        gameCompleteScreen.SetActive(true);
        scoreText.text = "x" + score;
        nonDiegeticSource.clip = gameComplete;
        nonDiegeticSource.loop = false;
        nonDiegeticSource.Play();
    }
    private void ResetScores()
    {
        score = 0;
        missed = 0;
    }

    //update UI in here
}
